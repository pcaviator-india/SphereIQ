# SphereIQ (Ezy-Nav) — macOS Apple Silicon (M-series) Porting Plan

**Status:** Planning only — no code changes yet.
**Date:** 2026-10-08
**Scope:** Port the SphereIQ / Ezy-Nav desktop application from Windows (x86, .NET Framework 2.0 WinForms) to macOS on Apple Silicon (arm64).

### Locked decisions (confirmed by stakeholder)
1. **Proprietary Ezy\* DLL sources will be obtained via ILSpy decompilation** (`..\ILSpy_windows_selfcontained_1.1.0.9782-x64` is available in the backup folder) — no need to hunt for original source.
2. **No licensing system in the ported app** — the product becomes completely free. CrypKey DRM and all licensing artifacts are **removed outright, not replaced**; no license validation code ships.
3. **Zero feature reduction** — every feature must survive the port (all 61 screens, Fax reports, USB-drive detection, file-type icons, charts, print/preview). Feature-cut options in this plan are void.
4. **Target framework: `net8.0` (LTS)** — CONFIRMED per recommendation (stability + LTS; Windows ships `net8.0-windows`, Mac ships `net8.0` + `osx-arm64` RID).
5. **UI framework: Avalonia 11.x** — CONFIRMED per recommendation (Option A).

**All Phase 0 decisions are now closed.** Remaining Phase 0 items are operational (Apple Developer account, repo hygiene, green baseline build).

---

## 1. Current-State Assessment (findings from code review)

### 1.1 Project shape
| Item | Value |
|---|---|
| Project file | `Ezy-Nav.csproj` (SDK-style, but `TargetFramework=net20`, `PlatformTarget=x86`, `UseWindowsForms=True`, `OutputType=WinExe`) |
| UI framework | Windows Forms (WinForms) — **Windows-only, even on modern .NET** |
| Source | 104 `.cs` files, ~34,350 lines |
| Screens | 61 form/screen classes (`Body_*`, `Popup_*`, `Toolbar_*`, `Form_*`, `Report_*`, `Splash`, `Layout_*`), 12 `Procedure_*` workflow classes, **73 `InitializeComponent()` blocks** |
| Resources | 5 `.resx` files (~1.4 MB total, mostly embedded images; `Form_MainForm.resx` 487 KB, `Splash.resx` 488 KB, `Resources.resx` 426 KB) |
| Entry point | `Program.cs` — `[STAThread]`, single-instance named `Mutex("PCAVIATOR SphereIQ")`, splash screen, `Application.Run` |
| Config | `app.config` with `System.Configuration` application/user settings (5 app settings, 1 user setting) |
| Build | `dotnet` SDK 10.0.401 available on this machine; `Microsoft.NETFramework.ReferenceAssemblies` used to build net20 |

### 1.2 Third-party / binary dependencies (in `..\rstudio\2iC Australia\Ezy-Nav\`)

| Assembly | Kind | WinForms/GDI bound? | Portability verdict |
|---|---|---|---|
| `EzyNavMath.dll` (2 files) | Proprietary, managed | **No** | ✅ Portable — pure math over `EzyPlanData`/`EzyPlanMath` types, zero Windows touches |
| `EzyPlanMath.dll` (34 files) | Proprietary, managed (VB-origin) | **No WinForms** — needs `Microsoft.VisualBasic` NuGet + `System.Configuration` (MySettings) | ✅ Portable once VB runtime package referenced; error-enum classes are plain logic |
| `EzyPlanStorage.dll` (`DrillHoleFile`, 2 files) | Proprietary, managed | **No WinForms — but uses `BinaryFormatter`** (`BinaryFormatter` deserialization of `.ezy` hole files) | ⚠️ Portable only after serialization migration: `BinaryFormatter` is **removed/blocked in net8** — must write a compat reader for legacy files + move to `System.Text.Json`/protobuf going forward |
| `EzyWizard.dll` (9 files) | Proprietary, managed | **No** — `IWizardForm`/`EzyWizard` orchestration is UI-free (no WinForms usings) | ✅ Portable as logic; Avalonia implements `IWizardForm` later |
| `LumenWorks.Framework.IO.dll` | OSS (CSV reader), managed | **No** | ✅ Portable (or swap to NuGet `LumenWorks.Framework.IO.Core`) |
| `EzyPlanData.dll` (29 files) | Proprietary, managed | **3 small touches only:** `DrillHoleStatus.MinimumDLStateColor` (`System.Drawing.Color`), `SortableBindingList<T>` (`BindingList<T>`+`ITypedList`), `Settings` (`System.Configuration`) | ✅ Portable after: `Color`→own struct/string, `SortableBindingList`→`ObservableCollection`+LINQ sort, settings→JSON |
| `EzyNavCharting.dll` (`EzyNavHoleChart` + controller, 6 files) | Proprietary, managed | **Yes — `NChartControl` + full WinForms controller UI** | ⚠️ Logic (chart data prep) separable; rendering must be reimplemented on OxyPlot |
| `EzyReport.dll` (11 files) | Proprietary, managed | **Yes — `Report : PrintDocument` (WinForms printing), `ReportViewer*` controls, `IReportTemplate.Controls` (`Control.ControlCollection`), PdfSharp 1.x** | ⚠️ Split required: report *models/templates* portable; `PrintDocument`/viewer replaced by PDFsharp-6/QuestPDF pipeline + Avalonia preview |
| `Nevron.System/Presentation/Chart(.WinForm)/GraphicsGL.dll` | Commercial 3rd party | **Yes — WinForms/OpenGL Windows** | ❌ Blocked — **no macOS build exists**; must be replaced (OxyPlot); existing `NLicense` activation code deleted |
| `PdfSharp.dll` (old v1.x) | OSS | **Yes (GDI+ / System.Drawing)** | ⚠️ Must upgrade to PDFsharp ≥6 (cross-platform) or QuestPDF |
| `Fixed RibbonMenuButton.dll`, `MultipleSelectionDataGridView.dll` | 3rd party WinForms controls | Yes | ❌ Must be replaced/reimplemented |
| `CrypKeyNET2.dll` + `crp32dll.dll`/`crp64DLL.dll` + `CKS.exe`/`SetupEx.exe`/`*.lic`/`*.ckn` | **Native Windows DRM/licensing (x86/x64 PE)** | Native Win32 | 🗑️ **Delete — app is free; no licensing to port or replace** |
| `Ezy-Nav.exe.licenses` | Compiled WinForms licenses blob | WinForms licensing | 🗑️ **Delete — obsolete, app is free** |

---

## 1.3 Windows-specific code in the app source

| Location | Windows dependency | Notes |
|---|---|---|
| `Dolinay\DriveDetector.cs` (+ 3 files) | `user32.dll` P/Invoke: `RegisterDeviceNotification`/`UnregisterDeviceNotification`, `WM_DEVICECHANGE`, volume structs | USB stick plug/unplug detection for file-open dialogs |
| `EzyPlan_Interface_V2\IconExtractor.cs` | `shell32.dll` P/Invoke: `SHGetFileInfo` (file-type icons) | Used by file list UIs |
| `Control_DriveBox.cs`, `DriveListElement.cs`, `FilesListBox.cs` | Drive-letter model (`C:\`, `SelectedPath = "c:\"`), Windows drive enumeration | Must map to macOS `/Volumes` + user folders |
| `Form_MainForm.cs` | `using Nevron;`, hard-coded `NLicense("6395a0bc-...")`, `NLicenseManager` | Nevron license activation |
| ~523 × `new Font("Microsoft Sans Serif", ...)` + 6 × `Tahoma` | Windows-only fonts | Will substitute; layout metrics differ on macOS |
| `Icon.FromHandle` usages | GDI icon handles | Needs replacement (SkiaSharp/ImageSharp) |
| `Program.cs` | Named `Mutex` single-instance | Works on modern .NET on macOS — verify |
| `app.config` / `Properties\Settings.cs` | `System.Configuration` (not in modern .NET by default) | Needs package or JSON migration |
| `Report_Fax*` files | Fax cover sheet report templates (**confirmed report-only**: just `FromFax`/`ToFax` string labels on the cover page, no fax-service API in app or DLLs) | **Portable — render as PDF** (visual parity, no transport) |
| `Popup_ReportPreview.cs`, `Report_*` | WinForms print preview + `EzyReport.Report : PrintDocument` | Re-express as PDF preview (PDFsharp 6/QuestPDF pipeline) |
| `EzyPlanStorage.DrillHoleFile` | **`BinaryFormatter` deserialization** of `.ezy` hole files (blocked in net8) | Compat reader for legacy files + new-format save; **blocks file open** until solved |
| `EzyPlanMath` (VB-origin) | `Microsoft.VisualBasic` + `MySettings` (`System.Configuration`) | `Microsoft.VisualBasic` NuGet + JSON settings |

### 1.4 Key conclusion

> **WinForms does not run on macOS** — not on .NET Framework, and not on modern .NET (WinForms is Windows-only in .NET 5+). There is no "retarget the TFM and ship" path. This is a **UI-framework migration**, not a recompile. Business logic (math, storage, wizard, CSV) is largely portable; with licensing **resolved by deletion** (app goes free), the two remaining hard problems are the **UI shell (61 screens)** and **charting (Nevron replacement)** — both must be solved while preserving **100% of features**.

---

## 2. Strategic Options (decision required before implementation)

### Option A — Avalonia UI (RECOMMENDED)
- Cross-platform (macOS arm64, Windows, Linux), XAML-based, mature, MIT-licensed, actively maintained.
- Closest modern analogue to WinForms' component model; supports `InitializeComponent`-style and MVVM.
- First-class arm64 support — native, no Rosetta; `dotnet publish` produces mac bundles directly.
- **Cost:** rewrite all 61 screens as Avalonia XAML; replace charts and 3rd-party controls.

### Option B — .NET MAUI (Mac Catalyst)
- Microsoft-supported, but macOS support is via **Mac Catalyst**, second-class for dense desktop tools (iOS-derived UX, awkward desktop file dialogs, menu-bar parity issues).
- **Not recommended** for SphereIQ's UI style.

### Option C — Keep WinForms, run in a Windows VM (Parallels/CrossOver)
- Not a real port; only a customer stopgap. Excluded from scope.

### Option D — Web/Blazor front end
- Viable long-term but a bigger architectural change (backend service, session state) than Avalonia for a local file/USB-centric desktop app.

**Recommendation: Option A (Avalonia)**, multi-targeting `net8.0-windows` + `net8.0-osx-arm64` so Windows keeps shipping from one codebase.

---

## 3. Dependency Replacement Plan

| Current | Replacement | Rationale |
|---|---|---|
| WinForms (61 screens) | **Avalonia 11.x** (`Avalonia.Desktop`, `Avalonia.Themes.Fluent`) | Cross-platform UI, native arm64 |
| Nevron chart suite (`EzyNavHoleChart`) | **OxyPlot.Avalonia** (MIT; alternatives: LiveCharts2, ScottPlot 5) | Nevron has no macOS target; reimplement the hole-trajectory chart surface |
| `EzyNavCharting.dll` | Decompile via ILSpy (tool already present in backup folder) → port → replace Nevron calls with OxyPlot | Source not in repo; DLL is WinForms-bound |
| `EzyPlanData.dll`, `EzyReport.dll` | Decompile, strip WinForms coupling, re-express report rendering | Required to build for macOS |
| `PdfSharp.dll` v1.x (GDI+) | **PDFsharp 6.x** or **QuestPDF** | v1.x depends on `System.Drawing`; v6 is cross-platform |
| `Fixed RibbonMenuButton.dll` | Avalonia `SplitButton`/custom ribbon control | WinForms-only |
| `MultipleSelectionDataGridView.dll` | Avalonia `DataGrid` + custom multi-select behavior | WinForms-only |
| `VistaStyleProgressBar` (in-repo) | Avalonia `ProgressBar` | In-repo WinForms control |
| CrypKey DRM (`crp32/64dll.dll`, `CKS.exe`) | **Nothing — delete entirely** | App is completely free: remove `CrypKeyNET2.dll`, `crp32dll.dll`, `crp64DLL.dll`, `CKS.exe`, `SetupEx.exe`, `ezyplan.lic`, `ezyplan.ckn`, `crp32002.ngn`, `Ezy-Nav.exe.licenses`, and any CrypKey invocation in decompiled source |
| `user32` drive notifications (`DriveDetector`) | macOS `DiskArbitration` framework P/Invoke, fallback = poll `/Volumes` / `FileSystemWatcher` | USB arrival/removal events |
| `shell32!SHGetFileInfo` icons (`IconExtractor`) | macOS `NSWorkspace.icon(forFile:)` via ObjC interop, or generic per-extension icons | File-type icons |
| `"Microsoft Sans Serif"` (523×), `"Tahoma"` (6×) | Central font tokens → **Helvetica Neue / SF Pro**; define in `App` theme resources | Fonts absent on macOS; metrics differ |
| `System.Configuration` settings | `System.Configuration.ConfigurationManager` NuGet **or** JSON settings in `~/Library/Application Support/SphereIQ/` | Not in modern .NET by default |
| Registry (used internally by Nevron) | Gone with Nevron removal | — |
| Drive letters (`C:\`, `c:\`) | `Environment.GetFolderPath(...)`, `/Volumes/*`, `~/Documents` | Path model |
| WinForms print preview | PDF export preview (PdfSharp already in stack) | Reports |
| `PlatformTarget x86` | Remove; publish `osx-arm64` (optionally `osx-x64` universal) | M-series target |

---

## 4. Proposed Target Architecture

```
SphereIQ.sln
├── src/
│   ├── SphereIQ.Core/            net8.0 — pure logic:
│   │                             (port EzyNavMath, EzyPlanMath, EzyPlanStorage,
│   │                              EzyWizard, CSV via LumenWorks, domain models)
│   ├── SphereIQ.Reports/         net8.0 — PDFsharp 6 / QuestPDF report generation
│   │                             (port EzyReport, de-WinForms it)
│   ├── SphereIQ.Charting/        net8.0 — OxyPlot-based hole-trajectory chart
│   │                             (replacement for EzyNavCharting/Nevron)
│   ├── SphereIQ.App/             Avalonia app, multi-target:
│   │                             - net8.0-windows   (keeps Windows working)
│   │                             - net8.0-osx-arm64 (M-series)
│   │                             - net8.0-osx-x64   (optional Intel Macs)
│   └── SphereIQ.Analyzers/       (optional) Roslyn analyzer enforcing
│                                 no WinForms refs in Core/Reports
├── tests/                        xUnit: Core math/storage/CSV golden-file tests
└── build/                        scripts: dotnet publish, .app bundle, DMG, signing
```

**Key principle:** keep **all business logic out of the UI layer** so the same core runs on Windows and macOS. The current code already splits `Procedure_*` (flow/logic) from `Body_*`/`Popup_*` (views) — preserve and strengthen that boundary during the port.


---

## 5. Phased Execution Plan

### Phase 0 — Preconditions & decisions (before any code)
> Items 3, 4, 5 below are **already resolved** by the locked decisions at the top of this document; kept for traceability.
1. **Decide target framework:** `net8.0` LTS (recommended) vs `net9.0`; SDK 10.0.401 on this machine builds either. — *still open*
2. **Confirm UI framework:** Avalonia (Option A). — *recommendation stands; confirm*
3. ~~Licensing decision~~ ✅ **Resolved:** app is completely free — CrypKey and all licensing artifacts are deleted, no replacement scheme, no `SphereIQ.Licensing` project.
4. ~~Source availability~~ ✅ **Resolved:** decompile `EzyNavCharting`, `EzyPlanData`, `EzyReport`, `EzyWizard` (and any other Ezy\* DLL needing changes) with ILSpy (`..\ILSpy_windows_selfcontained_1.1.0.9782-x64`).
5. ~~Feature inventory sign-off~~ ✅ **Resolved:** **zero feature reduction** — full feature parity is a hard requirement, including Fax reports, USB-drive detection, and shell file icons.
6. **Hardware/accounts:** Apple Silicon Mac for build & test; Apple Developer Program ($99/yr) for Developer ID signing + notarization (mandatory for distribution — Gatekeeper blocks unsigned apps regardless of price).
7. **Repo hygiene baseline:** add `.gitignore` for `bin/`, `obj/`, `.vs/` (build outputs are currently committed); verify current Windows build is green before starting.

**Exit criteria:** target framework + UI framework confirmed; baseline Windows build green; repo hygiene in place (licensing/source/feature questions already closed).

### Phase 1 — Decouple logic from UI (on Windows, no visual changes)
1. Extract logic into `SphereIQ.Core` / `SphereIQ.Reports` class libraries (net8.0, zero WinForms references).
2. Introduce abstractions for Windows-only services:
   - `IFileIconService` (replaces `IconExtractor`)
   - `IRemovableMediaMonitor` (replaces `DriveDetector`)
   - `IFileDialogService` (replaces drive-letter `FilesListBox`/`DriveBox` browsing)
   - Central font tokens (replaces 523 hard-coded `"Microsoft Sans Serif"`)
   - `ISettingsStore` (replaces `System.Configuration`)
   - `IPdfExportService`
3. Add unit tests around extracted logic (golden files: CSV import, survey math, extrapolation, build-to-target) — the safety net for the UI rewrite.
4. Cross-platform proof: build Core/Reports with `dotnet build -r osx-arm64` (logic-only assemblies must compile for non-Windows).

**Exit criteria:** Core/Reports compile with no `System.Windows.Forms` reference; tests pass; Windows app still works (still WinForms at this point).
✅ **Phase 1 DONE (commits `12574be`, `5949276`):** `SphereIQ.Core` (data+math+storage+Wizard logic, net8.0, 0 errors, JSON `.ezn` format) + `SphereIQ.Reports` (report/chart models) + 9 xUnit tests green; legacy `.ezn` BinaryFormatter detected via magic header with converter guidance.

### Phase 2 — Replace Windows-only dependencies
0. **Decompile first:** run ILSpy on `EzyNavCharting.dll`, `EzyPlanData.dll`, `EzyReport.dll`, `EzyWizard.dll` → import as source projects (`SphereIQ.Charting`, `SphereIQ.Core` parts) → **strip all CrypKey/licensing calls and any residual WinForms coupling** during import.
1. **Charts:** inventory Nevron API surface from decompiled `EzyNavCharting` → prototype OxyPlot replacement for `EzyNavHoleChart` covering the 4 context-menu views in `Form_MainForm` (`dView`, `eastingProfile`, `northingProfile`, `topProfile`).
2. **Reports:** upgrade PdfSharp 1.x → PDFsharp 6 (API rework around `XPen`/`XGraphics`) or migrate to QuestPDF; port decompiled `EzyReport` + `Report_*` classes to render PDFs without WinForms graphics; re-check `Popup_ReportPreview`.
3. **Controls:** replace `RibbonMenuButton` and multi-select grid with Avalonia equivalents (small `SphereIQ.UI` library: `RibbonButton`, `MultiSelectDataGrid`).
4. **Fonts:** font map + layout re-tuning (WinForms points ≠ Avalonia DIPs; test at multiple DPI scales).
5. **Licensing removal (not replacement):** delete CrypKey artifacts (`CKS.exe`, `SetupEx.exe`, `crp32dll.dll`, `crp64DLL.dll`, `CrypKeyNET2.dll`, `crp32002.ngn`, `ezyplan.lic`, `ezyplan.ckn`, `Ezy-Nav.exe.licenses`) from the repo/distribution, remove the Nevron `NLicense`/`NLicenseManager` activation lines, and grep decompiled sources for any CrypKey invocation to ensure nothing references them at runtime. No validation code ships — the app is free.

**Exit criteria:** no Nevron, no CrypKey, no PdfSharp-1.x, no WinForms-only controls in the dependency graph (`dotnet list package` audit clean); zero references to any licensing code in source or output.
✅ **Phase 2 DONE (commits `f49fa4f`, `b84c090`):** `HoleChartData` ports all 13 Nevron series 1:1 (set/visibility semantics, X=Easting/Y=TVD/Z=Northing, ±20M range guards, view projection) + typed builders for every `Report_*` template; PDFsharp 6 pipeline with cross-platform font resolver + `FontMap` (MS Sans Serif → Helvetica Neue, pt→DIP); zero `System.Drawing`/`WinForms`/`Nevron`/`PdfSharp-1.x`/`CrypKey` references in `src/`; 16 tests green.

### Phase 3 — Avalonia UI port (bulk of the work)
Strategy: **screen-by-screen vertical slices**, not a big-bang rewrite.

> 🚧 **In progress (commit `8b30682`):** Avalonia `SphereIQ.App` scaffold is live — single-instance mutex, `AppSession`, `ScreenFactory`, back-stack `MainWindow` shell. Native file picker replaces the Windows drive-box/list (`StorageProvider.OpenFilePickerAsync` → Finder on macOS / Explorer on Windows), filtered to `.ezn`; on `NoError` it opens via `DrillHoleFile.Open`. Hole-file format migrated to JSON. All business logic is ported UI-free into `SphereIQ.Reports`: `HoleChartData` (all chart `Set*` series), `ReportDocument`/`ReportPdfRenderer` (PDF via PdfSharp 6), and `HolePlanningEngine` (1:1 port of `DisplayCalculatedData` incl. MinDL state + sidetrack branching). OxyPlot `ChartHost` renders `HoleChartData`, replacing Nevron. Remaining screens render as `PlaceholderScreen` so navigation never dead-ends. Build green, 19 tests pass.

**Slice tracker:**
- [x] Slice 1: Splash + Welcome
- [x] Slice 2: Open file (native file picker; Create file TBD)
- [x] Create New Hole wizard (3 steps + native save picker)
- [x] Charting foundation: `HoleChartData` + OxyPlot `ChartHost` + `HolePlanningEngine` (logic, UI-free, tested)
- [x] Slice 3: Hole management (+ toolbar)
- [x] Slice 4 (screen): Hole planning screen wired to `HolePlanningEngine` + `ChartHost` + result fields (`DrillHolePlanningScreen`)
- [x] Slice 5: Edit survey data (grids — heaviest control port; `EditSurveyDataScreen` compiles, 19 tests green)
- [x] Slice 7 (part 1): Build To Target screen (`BuildToTargetScreen`, select + results views + chart)
- [x] Slice 4 (popups): Trend Analysis screen (`TrendAnalysisScreen`, range select + results + chart)
- [x] Slice 4 (popups): Extrapolate screen (`ExtrapolateScreen`, entry + results + chart)
- [ ] Slice 4 (popups): Sidetrack popup
- [ ] Slice 6: Import CSV flows (exercises `ICsvReader` + `Body_Import_*`)
- [ ] Slice 7 (part 2): Side track, Edit details screens (currently `PlaceholderScreen`)
- [ ] Slice 8: Reports & preview (`Popup_ReportPreview`, `Popup_Report_*`, Fax screens — **mandatory, zero feature reduction**)
- [ ] Slice 9: Misc popups (message boxes, password, working indicator)

1. **Scaffold** `SphereIQ.App` (Avalonia + service wiring); port `Program.Main` (single-instance mutex, splash, global exception handlers).
2. **Port shell first:** `Form_MainForm` → Avalonia `MainWindow` (header, toolbar, body/info/fill panels, screen title, chart region, context menus).
3. **Port by workflow** (one `Procedure_*` per slice, with its `Body_*`/`Popup_*`/`Toolbar_*` views):
   - Slice 1: Splash + Welcome
   - Slice 2: Open/Create file (exercises file dialogs & drive UI)
   - Slice 3: Hole management (+ toolbar)
   - Slice 4: Hole planning (+ advanced popups)
   - Slice 5: Edit survey data (grids — heaviest control port)
   - Slice 6: Import CSV flows (exercises LumenWorks + `Body_Import_*`)
   - Slice 7: Build to target, Extrapolate, Trend analysis, Side track, Edit details (results screens + charts)
   - Slice 8: Reports & preview (`Popup_ReportPreview`, `Popup_Report_*`, Fax screens — **mandatory, zero feature reduction**)
   - Slice 9: Misc popups (message boxes, password, working indicator)
4. **Layout technique:** start with absolute positioning (Avalonia supports WinForms-like pixel geometry) to preserve visuals; optionally migrate to responsive `Grid`/`DockPanel` later — not required for v1.
5. **Resources:** convert `.resx` embedded images to Avalonia assets (`avares://`); use existing `SphereIQ.svg` to produce the macOS `.icns` icon.

**Per-slice definition of done:** builds for macOS, renders correctly on M-series, workflow passes manual test script, no new WinForms references.

**Exit criteria:** all 61 screens functional in Avalonia on macOS arm64.

### Phase 4 — macOS platform integration
> 🚧 **Groundwork done (platform-independent part only), commit TBD.** Three
> items are built and verifiable from Windows because they don't depend on the
> unfinished UI: **(1)** `build/Info.plist` — bundle id `com.sphereiq.EzyPlan`,
> `LSMinimumSystemVersion` 11.0, `NSHighResolutionCapable`, plus
> `.ezn` document-type association via exported UTI `com.sphereiq.ezn`
> (wrapped by `build/make-app.sh`, `bash -n` clean, LF endings);
> **(3)** native open/save panels already use Avalonia's
> `StorageProvider` (Avalonia picks `NSOpenPanel` on macOS automatically, so
> no new code needed); **(7)** `CrashLog` writes to
> `~/Library/Application Support/SphereIQ/Logs/sphereiq.log` on Unix and
> `%APPDATA%\SphereIQ\Logs` on Windows, and routes
> `AppDomain.UnhandledException`, `TaskScheduler.UnobservedTaskException`,
> `Dispatcher.UIThread.UnhandledException` and `Avalonia.Logging.Logger`
> (Warning+) into it — this is the substitute for a debugger, since the dev
> machine is Windows-only and Finder-launched apps discard stdout.
> `VersionPrefix` is now the single version source for assembly, `Info.plist`
> and the log banner.
>
> **Verified from Windows:** `dotnet publish -r osx-arm64 --self-contained true
> -c Release` succeeds → 236 files / 105.5 MB including `libAvaloniaNative.dylib`,
> `libSkiaSharp.dylib` and the arm64 `coreclr`/`clrjit`, so cross-compilation
> and the native dependency graph are proven. **Not yet verified:** actual
> launch, menus (item 2), DiskArbitration (item 4), `NSWorkspace` icons
> (item 5), Dock behaviour (item 8) and Retina rendering (item 9) — all need
> the Apple Silicon Mac. Items 2/4/5 also still need the `ISettingsStore`,
> `IRemovableMediaMonitor` and `IFileIconService` interfaces that Phase 1 §2
> specified and which do not exist in `src/` yet. Shipped as commit `6415af2`.
>
> **To test on the Mac:**
> ```
> ./build/make-app.sh                      # packages artifacts/publish/osx-arm64
> artifacts/app/SphereIQ.app/Contents/MacOS/SphereIQ   # run from Terminal
> cat ~/Library/Application\ Support/SphereIQ/Logs/sphereiq.log
> ```
> The `[env] SphereIQ 1.0.0+<sha> | runtime ... | procarch Arm64 ...` banner
> confirms the arm64 runtime started. The script only packages — `dotnet
> publish` must have run first (on Windows, or on the Mac with the arm64 SDK).

1. **App bundle:** produce `SphereIQ.app` with `Info.plist` (bundle id, version from `Settings.ProductVersion`, `LSMinimumSystemVersion`, document-type associations for the drill-hole file extension, `NSHighResolutionCapable` Retina keys).
2. **Menus:** native macOS menu bar — app menu (About/Preferences/Quit), Edit menu (Cmd+C/V/Z must work), Window menu; convert `Ctrl` shortcuts to `Cmd`.
3. **Dialogs:** native open/save panels (macOS `NSOpenPanel` via Avalonia desktop dialogs); sandbox-friendly paths.
4. **Removable media:** DiskArbitration-based mount/unmount notifications behind `IRemovableMediaMonitor`; show `/Volumes` entries in file browsers; fallback = polling. Note: macOS has no veto equivalent of `BROADCAST_QUERY_DENY` — degrade to warning only.
5. **File icons:** `IFileIconService` macOS implementation (`NSWorkspace.icon(forFile:)`) or generic per-extension icons.
6. **Single instance + file association:** keep mutex behavior; activate existing window on second launch; handle `OpenFile` events for double-click open.
7. **Paths:** user data → `~/Library/Application Support/SphereIQ/`, logs → `~/Library/Logs/SphereIQ/`, settings → JSON (one-time migration from old `app.config` values).
8. **Dock:** app icon, badge, correct minimize/restore behavior.
9. **Font/visual check:** verify every screen at Retina 2x and on non-Retina with substituted fonts.

**Exit criteria:** app feels native on macOS (menus, dialogs, paths, shortcuts, dock).

### Phase 5 — Packaging, signing, distribution
1. `dotnet publish -c Release -r osx-arm64 --self-contained true` (no .NET install required by users).
2. Bundle → `codesign --deep --force --options runtime` with Developer ID Application certificate.
3. Notarization (`notarytool submit --wait`) + stapling — **mandatory** or Gatekeeper blocks the app.
4. Produce DMG (e.g., `create-dmg`) and/or `.pkg`.
5. Optional: `osx-x64` build + `lipo` universal binary for Intel Macs.
6. Windows channel: keep shipping via `net8.0-windows` target of the same codebase; decide release cadence.

**Exit criteria:** signed, notarized DMG installs and runs on a clean M-series Mac.

### Phase 6 — Test & validation matrix
| Area | Tests |
|---|---|
| Core logic | Unit tests (Phase 1): survey math, extrapolation, build-to-target, trend analysis, CSV golden files |
| UI regression | Manual script per workflow × (macOS arm64, Windows); screenshot comparison vs current Windows build |
| Reports | PDF visual compare vs Windows baseline for each `Report_*` type |
| Charts | Visual parity for all 4 context-menu views |
| USB/media | Plug/unplug during file dialog; file-lock behavior (`DriveDetector.OpenedFile`) |
| Settings | Persist across restart; migration of old `app.config` values |
| Edge | Single-instance launch, file-association double-click, corrupted-file open errors, password popup, 100–200% DPI scaling |
| Perf | Cold start < 3 s on M1; chart redraw responsiveness |

**Exit criteria:** parity checklist signed off; no open critical/major bugs.

---

## 6. Risks & Mitigations

| # | Risk | Severity | Mitigation |
|---|---|---|---|
| R1 | **Nevron → OxyPlot chart parity** (trajectory visualization is core product value) | High | Decompile `EzyNavCharting` first to scope the exact API surface; prototype the chart in Phase 2 before mass UI porting |
| R2 | **No source for proprietary DLLs** (`EzyPlanData`, `EzyReport`, `EzyNavCharting`) | ~~High~~ **Resolved** | ILSpy decompilation confirmed as the source of truth (tool on hand); budget cleanup pass — decompiled code needs naming/formatting normalization but is functionally complete |
| R3 | ~~CrypKey DRM has no macOS equivalent~~ | ~~High~~ **Resolved** | App is completely free — CrypKey is deleted, no replacement needed |
| R4 | Layout drift: 523 hard-coded WinForms fonts + absolute pixel layouts under macOS font metrics/Retina | Medium | Central font tokens; explicit per-screen visual QA; accept minor pixel differences, avoid "pixel-perfect" gate |
| R5 | Scope: 61 screens with hand-coded `InitializeComponent` (73 blocks) — large manual effort | Medium | Slice-based delivery; consider conversion tooling for initial mapping; keep absolute layout initially |
| R6 | Avalonia DataGrid multi-select gap (replaces `MultipleSelectionDataGridView`) | Medium | Build custom selection behavior early (Slice 5 dependency) |
| R7 | USB safe-removal semantics differ on macOS (no `WM_DEVICECHANGE` veto) | Low | Degrade gracefully: warn-on-unmount only, no veto |
| R8 | Apple notarization/signing unfamiliar | Low | Budget Apple Developer account + CI signing step; verify early with a hello-world bundle |
| R9 | Old net20 API/behavior differences (`Mutex`, `Thread`, `STAThread`) under modern .NET | Low | Phase 1 compile pass surfaces these; `[STAThread]` irrelevant in Avalonia |
| R10 | Fax feature may depend on Windows fax services | Medium | **Feature cannot be cut** — analyze decompiled `EzyReport` fax path in Phase 1; expected to be a printable/PDF cover sheet (portable); if it calls a Windows fax API, replicate output as PDF only (visual parity, not transport) |

### 6.1 Additional note — zero-reduction constraint
"No feature reduction" raises the bar on three areas that a cheaper port could have skipped: **(a)** Fax report screens must render identically as PDFs, **(b)** USB plug/unplug detection must genuinely work on macOS (DiskArbitration path, not stubbed), **(c)** file-type icons must come from the OS (NSWorkspace), not generic placeholders. All three are covered by Phase 4 items 4–5 and verified in the Phase 6 matrix.

---

## 7. Rough Effort Indication (planning only, not a commitment)

| Phase | Relative effort |
|---|---|
| Phase 0 decisions | S (days) |
| Phase 1 logic extraction + tests | M (1–2 weeks) |
| Phase 2 dependency replacement (charts/reports/licensing) | **L (2–4 weeks — highest uncertainty)** |
| Phase 3 Avalonia port of 61 screens | **XL (6–10 weeks — bulk of project)** |
| Phase 4 macOS integration | M (1–2 weeks) |
| Phase 5 packaging/signing | S (days) |
| Phase 6 QA/parity | M (1–2 weeks) |
| **Total** | **~3–5 months elapsed for one experienced developer** |

---

## 8. Immediate Next Actions
1. ~~Confirm the two open Phase 0 items~~ **DONE — locked:** `net8.0` LTS + Avalonia 11.x.
2. Acquire an Apple Silicon Mac + enroll in the Apple Developer Program (needed for notarization even for a free app).
3. ~~Set up repo hygiene (`.gitignore`) and confirm a green baseline Windows build.~~ **DONE — `.gitignore`, baseline commit `132be11` (green net20 build).**
4. ~~Begin Phase 1 extraction (`SphereIQ.Core`) with unit tests~~ **DONE —** `SphereIQ.Core` (net8.0, 0 errors, JSON `.ezn` format) + `SphereIQ.Reports` + 19 xUnit tests green; legacy `.ezn` BinaryFormatter detected via magic header with converter guidance.
5. ~~Run ILSpy reconnaissance~~ **DONE.**
6. **Build green** — `dotnet build src/SphereIQ.App` succeeds (1 cosmetic warning); `dotnet test` → 19/19 passed.
7. **Continue Phase 3 slice work:**
   - Slice 4 popups: Sidetrack popup (currently `PlaceholderScreen`)
   - Slice 6: Import CSV flows
   - Slice 7 (part 2): Side track, Edit details screens (currently `PlaceholderScreen`)
   - Slice 8: Reports & preview (mandatory, zero feature reduction)
8. **Phase 4 interfaces still missing:** `ISettingsStore`, `IRemovableMediaMonitor`, `IFileIconService` — create these in `SphereIQ.Core` before macOS integration.

---
*This plan was produced by static code/dependency analysis only; no application code was modified.*

