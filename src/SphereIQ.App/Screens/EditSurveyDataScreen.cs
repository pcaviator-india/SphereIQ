using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using EzyNavMath;
using EzyPlanData;
using EzyPlanStorage;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Phase 3 slice 5: Edit Survey Data. Port of the legacy
    /// <c>Procedure_EditSurveyData</c> + <c>Body_EditSurveyData</c> pair.
    /// <para>
    /// The grid is a read-only <see cref="DataGrid"/> bound to <see cref="SurveyRow"/>
    /// wrappers, mirroring the legacy <c>EditMode = EditProgrammatically</c>: all
    /// mutation goes through the Add / Edit / Remove / Insert popups, never by
    /// typing into a cell. The DL severity colouring, the
    /// Severe/Extreme colour key and the multi-select semantics of
    /// <c>SelectionMode = FullRowSelect</c> are preserved.
    /// </para>
    /// <para>
    /// Edits are held in a private copy of the survey list
    /// (<c>new SurveyList(file.SurveyList)</c>, as the legacy
    /// <see cref="Body_EditSurveyData"/> did) and only written back to the hole
    /// file on Save, so Cancel discards them.
    /// </para>
    /// <para>
    /// Import is out of scope for this slice (legacy Import_SelectFile /
    /// SelectMap / CreateMap is slice 6); the Import button navigates to
    /// <see cref="ScreenId.Import"/> instead of doing the work inline.
    /// </para>
    /// </summary>
    public sealed class EditSurveyDataScreen : IScreen
    {
        // Matches HolePlanningEngine's defaults, which is what the drill-hole
        // planning screen used for the same thresholds.
        private const decimal DefaultSevereDL = 3m;
        private const decimal DefaultExtremeDL = 7m;

        private readonly AppSession _session;
        private SurveyList _surveyData;
        private readonly ObservableCollection<SurveyRow> _rows = new ObservableCollection<SurveyRow>();
        private AbsoluteSurveyList _absolute = new AbsoluteSurveyList();

        private DataGrid _grid;
        private Button _save;
        private Button _back;

        public ScreenId Id { get { return ScreenId.EditSurveyData; } }

        public object Content { get; private set; }

        public event EventHandler<NavigateEventArgs> Navigate;

        public AppSession Session { get { return _session; } set { } }

        public EditSurveyDataScreen(AppSession session)
        {
            _session = session;
            Build();
        }
        private void Build()
        {
            Grid root = new Grid();
            root.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            root.RowDefinitions.Add(new RowDefinition(GridLength.Star));

            root.Children.Add(BuildToolbar());
            Grid.SetRow(root.Children[0], 0);

            root.Children.Add(BuildBody());
            Grid.SetRow(root.Children[1], 1);

            Content = root;
        }

        // Toolbar order follows the legacy ribbon: Home, Edit Details, Edit
        // Survey Data (current), Import, Hole Planning.
        private StackPanel BuildToolbar()
        {
            StackPanel bar = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 6,
                Margin = new Thickness(12, 8)
            };

            bar.Children.Add(Button("Home", ScreenId.Welcome));
            bar.Children.Add(Button("Edit Details", ScreenId.EditDetails));
            bar.Children.Add(Button("Edit Survey Data", ScreenId.EditSurveyData));
            bar.Children.Add(Button("Import Survey Data", ScreenId.Import));
            bar.Children.Add(Button("Hole Planning", ScreenId.HolePlanning));

            return bar;
        }

        private Button Button(string text, ScreenId destination)
        {
            Button b = new Button
            {
                Content = text,
                Margin = new Thickness(0, 0, 4, 0),
                Padding = new Thickness(12, 6),
                IsEnabled = destination != ScreenId.EditSurveyData
            };
            b.Click += (s, e) => Raise(destination);
            return b;
        }

        private Grid BuildBody()
        {
            Grid body = new Grid { Margin = new Thickness(12, 0, 12, 12) };
            body.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            body.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            body.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            body.Children.Add(BuildHeader());
            Grid.SetRow(body.Children[0], 0);

            body.Children.Add(BuildGrid());
            Grid.SetRow(body.Children[1], 1);

            body.Children.Add(BuildActionBar());
            Grid.SetRow(body.Children[2], 2);

            return body;
        }

        // label_headerText1 + the two usage notes from InitializeComponent.
        private StackPanel BuildHeader()
        {
            StackPanel header = new StackPanel { Spacing = 4, Margin = new Thickness(0, 4, 0, 8) };
            header.Children.Add(new TextBlock
            {
                Text = "Manually enter or edit the survey data:",
                FontSize = 15,
                FontWeight = FontWeight.Bold
            });
            header.Children.Add(new TextBlock
            {
                Text = "Dip can be entered as mineral or oil and gas standard but is stored as oil and gas.",
                FontSize = 12,
                Opacity = 0.75
            });
            header.Children.Add(new TextBlock
            {
                Text = "The Azimuth values must be relative to grid north. (Corrected for magnetic delination).",
                FontSize = 12,
                Opacity = 0.75
            });
            header.Children.Add(BuildSeverityKey());
            return header;
        }

        // panel_Key: the Severe/Extreme colour swatches with their captions.
        private StackPanel BuildSeverityKey()
        {
            StackPanel key = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 6,
                Margin = new Thickness(0, 6, 0, 0)
            };

            key.Children.Add(new Border
            {
                Width = 16,
                Height = 16,
                Background = Brushes.Orange,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1)
            });
            key.Children.Add(KeyLabel("Severe dogleg"));

            key.Children.Add(new Border
            {
                Width = 16,
                Height = 16,
                Background = Brushes.Red,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(12, 0, 0, 0)
            });
            key.Children.Add(KeyLabel("Extreme dogleg"));

            return key;
        }

        private static TextBlock KeyLabel(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontSize = 12,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        // Column order + headers mirror the legacy auto-generated columns from
        // the AbsoluteSurveyList.Data names (MD, Dip, Azimuth, TVD, Northing,
        // Easting, DL). IsReadOnly because the legacy grid was
        // EditMode.EditProgrammatically - editing happens in popups.
        private DataGrid BuildGrid()
        {
            _grid = new DataGrid
            {
                ItemsSource = _rows,
                IsReadOnly = true,
                AutoGenerateColumns = false,
                CanUserSortColumns = true,
                SelectionMode = DataGridSelectionMode.Extended,
                HeadersVisibility = DataGridHeadersVisibility.Column,
                GridLinesVisibility = DataGridGridLinesVisibility.All
            };

            _grid.Columns.Add(new DataGridTextColumn { Header = "MD", Binding = new Avalonia.Data.Binding("MD") });
            _grid.Columns.Add(new DataGridTextColumn { Header = "Dip", Binding = new Avalonia.Data.Binding("Dip") });
            _grid.Columns.Add(new DataGridTextColumn { Header = "Azimuth", Binding = new Avalonia.Data.Binding("Azimuth") });
            _grid.Columns.Add(new DataGridTextColumn { Header = "TVD", Binding = new Avalonia.Data.Binding("TVD") });
            _grid.Columns.Add(new DataGridTextColumn { Header = "Northing", Binding = new Avalonia.Data.Binding("Northing") });
            _grid.Columns.Add(new DataGridTextColumn { Header = "Easting", Binding = new Avalonia.Data.Binding("Easting") });
            _grid.Columns.Add(new DataGridTextColumn { Header = "DL", Binding = new Avalonia.Data.Binding("DL") });

            return _grid;
        }

        // panel_buttonPanel: Save / Back / Cancel. The legacy grid buttons
        // (Add New, Edit Selected, Remove Selected, Copy) live in a strip above
        // the action bar because they act on the current selection.
        private StackPanel BuildActionBar()
        {
            StackPanel root = new StackPanel { Margin = new Thickness(0, 8, 0, 0), Spacing = 8 };

            StackPanel editRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
            editRow.Children.Add(ActionButton("Add New", OnAddNew));
            editRow.Children.Add(ActionButton("Edit Selected", OnEdit));
            editRow.Children.Add(ActionButton("Remove Selected", OnRemove));
            editRow.Children.Add(ActionButton("Copy", OnCopy));
            root.Children.Add(editRow);

            StackPanel saveRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
            _save = ActionButton("Save", OnSave);
            _back = ActionButton("Back", OnBack);
            saveRow.Children.Add(_save);
            saveRow.Children.Add(_back);
            saveRow.Children.Add(ActionButton("Cancel", OnCancel));
            root.Children.Add(saveRow);

            return root;
        }

        private static Button ActionButton(string text, Action onClick)
        {
            Button b = new Button { Content = text, Padding = new Thickness(12, 6) };
            b.Click += (s, e) => onClick();
            return b;
        }

        /// <summary>
        /// Port of <c>Body_EditSurveyData.UpdateDataGrid</c>: recompute the
        /// absolute survey list from the collar + working copy of the survey data,
        /// then rebuild the bound rows. The legacy version re-coloured existing
        /// rows from cell 6; here the colour comes from
        /// <see cref="SurveyRow.Severity"/> on rebuild, which is equivalent
        /// because every row is recreated.
        /// </summary>
        private void UpdateDataGrid()
        {
            _absolute.Clear();
            if (_surveyData != null && HoleFile() != null)
            {
                NavMath navMath = new NavMath();
                navMath.AbsoluteSurvey(HoleFile().Collar, _surveyData, ref _absolute);
            }

            _rows.Clear();
            if (_absolute != null)
            {
                foreach (AbsoluteSurvey survey in _absolute)
                {
                    _rows.Add(new SurveyRow(survey, DefaultSevereDL, DefaultExtremeDL));
                }
            }
        }

        /// <summary>Refreshes the grid in place after an in-place edit.</summary>
        private void RefreshRows()
        {
            foreach (SurveyRow row in _rows)
            {
                row.Refresh();
            }
        }

        // --- DataModified / ResetDataModified / EnableBack / DisableBack ---
        // Legacy behaviour: Save becomes visible and Back appears once the data
        // has been touched.
        private void DataModified()
        {
            if (_save != null)
            {
                _save.IsVisible = true;
            }
        }

        private void ResetDataModified()
        {
            if (_save != null)
            {
                _save.IsVisible = false;
            }
        }

        private void EnableBack()
        {
            if (_back != null)
            {
                _back.IsVisible = true;
            }
        }

        private void DisableBack()
        {
            if (_back != null)
            {
                _back.IsVisible = false;
            }
        }

        // --- Selection helpers ---

        /// <summary>Indices of the selected rows, in grid order.</summary>
        private List<int> SelectedIndices()
        {
            List<int> indices = new List<int>();
            if (_grid == null || _grid.SelectedItems == null)
            {
                return indices;
            }
            foreach (object item in _grid.SelectedItems)
            {
                int index = _rows.IndexOf((SurveyRow)item);
                if (index >= 0)
                {
                    indices.Add(index);
                }
            }
            indices.Sort();
            return indices;
        }

        // --- Body_EditSurveyData.Edit / Append / Insert / Remove / Save ---

        /// <summary>Edit: legacy refused multi-select ("You can't edit multiple surveys!").</summary>
        private async void OnEdit()
        {
            List<int> indices = SelectedIndices();
            if (indices.Count == 0)
            {
                return;
            }
            if (indices.Count > 1)
            {
                await Dialogs.ShowMessageAsync(Owner(), "You can't edit multiple surveys!", "Select a single survey to edit.");
                return;
            }

            int index = indices[0];
            // Work on a copy so a cancelled dialog leaves the survey untouched.
            Survey working = new Survey(_surveyData.RecordAt(index));
            SurveyEditResult result = SurveyEditDialog.Edit(OwnerWindow(), working);
            if (!result.Confirmed)
            {
                return;
            }

            SurveyList.UpdateSurveyAtEnum update = _surveyData.UpdateSurveyAt(index, result.Survey);
            if (update == SurveyList.UpdateSurveyAtEnum.UpdateSuccessful)
            {
                UpdateDataGrid();
                SelectRowAndScrollTo(index);
                DataModified();
            }
            else
            {
                await Dialogs.ShowMessageAsync(Owner(), "Could not save changes!", SurveyList.ToHumanReadable(update) + ".");
            }
        }

        /// <summary>Add New - opens the Append popup (legacy context menu offered Append or Insert).</summary>
        private void OnAddNew()
        {
            StartAddLoop(SurveyEditDialog.Append(OwnerWindow()), -1);
        }

        /// <summary>Insert above the selected row; legacy required exactly one selected row.</summary>
        private async void OnInsert()
        {
            List<int> indices = SelectedIndices();
            if (indices.Count != 1)
            {
                await Dialogs.ShowMessageAsync(Owner(), "Insert survey", "Select a single survey to insert above.");
                return;
            }
            StartAddLoop(SurveyEditDialog.Insert(OwnerWindow()), indices[0]);
        }

        /// <summary>
        /// Drives the "Add and enter another" loop, mirroring
        /// <c>Append_Add</c> / <c>Append_AddFinish</c> (and the Insert pair).
        /// Deliberately async void: the loop awaits modal dialogs and is only
        /// ever invoked from a click handler.
        /// </summary>
        private async void StartAddLoop(SurveyEditResult first, int insertIndex)
        {
            SurveyEditResult result = first;
            while (result != null && result.Confirmed)
            {
                if (await ApplyAdd(result, insertIndex))
                {
                    DataModified();
                }
                if (!result.AddAnother)
                {
                    break;
                }
                result = insertIndex < 0
                    ? SurveyEditDialog.Append(OwnerWindow())
                    : SurveyEditDialog.Insert(OwnerWindow());
            }
            SelectLastRow();
        }

        /// <summary>Applies one add; returns false (after warning) on a validation error.</summary>
        private async System.Threading.Tasks.Task<bool> ApplyAdd(SurveyEditResult result, int insertIndex)
        {
            bool ok;
            if (insertIndex < 0)
            {
                SurveyList.AppendErrorsEnum append = _surveyData.Append(result.Survey);
                ok = append == SurveyList.AppendErrorsEnum.NoError;
                if (!ok)
                {
                    await Dialogs.ShowMessageAsync(Owner(), "Could not append new survey!", SurveyList.ToHumanReadable(append) + ".");
                }
            }
            else
            {
                SurveyList.InsertErrorsEnum insert = _surveyData.Insert(insertIndex, result.Survey);
                ok = insert == SurveyList.InsertErrorsEnum.NoError;
                if (!ok)
                {
                    await Dialogs.ShowMessageAsync(Owner(), "Could not insert new survey!", SurveyList.ToHumanReadable(insert) + ".");
                }
            }

            if (ok)
            {
                UpdateDataGrid();
            }
            return ok;
        }

        /// <summary>Remove: legacy confirmed with a Yes/No box before deleting.</summary>
        private async void OnRemove()
        {
            List<decimal> mds = SelectedMDs();
            if (mds.Count == 0)
            {
                return;
            }

            bool confirmed = await Dialogs.ShowConfirmAsync(Owner(), "Delete Selected Survey(s)", "Are you sure you want to delete the selected survey(s)?");
            if (!confirmed)
            {
                return;
            }

            SurveyList.RemoveEnum remove = _surveyData.Remove(mds);
            if (remove == SurveyList.RemoveEnum.RemoveSuccessfull)
            {
                UpdateDataGrid();
                DataModified();
            }
            else
            {
                await Dialogs.ShowMessageAsync(Owner(), "Remove Error!", SurveyList.ToHumanReadable(remove) + ".");
            }
        }

        /// <summary>
        /// Copy: legacy used DataGridView.GetClipboardContent (tab-delimited).
        /// Avalonia has no equivalent, so the same TSV shape is built by hand.
        /// </summary>
        private async void OnCopy()
        {
            List<int> indices = SelectedIndices();
            if (indices.Count == 0)
            {
                return;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("MD\tDip\tAzimuth\tTVD\tNorthing\tEasting\tDL");
            foreach (int index in indices)
            {
                AbsoluteSurvey s = _absolute.RecordAt(index);
                sb.Append(Environment.NewLine);
                sb.Append(s.MD.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append('\t');
                sb.Append(s.Dip.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append('\t');
                sb.Append(s.Azimuth.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append('\t');
                sb.Append(s.TVD.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append('\t');
                sb.Append(s.Northing.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append('\t');
                sb.Append(s.Easting.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append('\t');
                sb.Append(s.DL.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            Control owner = Owner();
            if (owner != null)
            {
                var top = Avalonia.Controls.TopLevel.GetTopLevel(owner);
if (top != null)
{
    await top.Clipboard.SetTextAsync(sb.ToString());
}
            }
        }

        /// <summary>
        /// Save: writes the working copy back to the hole file and saves it -
        /// <c>Body_EditSurveyData.Save</c> + <c>Procedure_EditSurveyData.SaveChangesToFile</c>.
        /// </summary>
        private async void OnSave()
        {
            DrillHoleFile file = HoleFile();
            if (file == null)
            {
                return;
            }

            file.SurveyList = _surveyData;
            DrillHoleFile.Save_ErrorsEnum save = file.Save();
            if (save != DrillHoleFile.Save_ErrorsEnum.NoError)
            {
                await Dialogs.ShowMessageAsync(Owner(), "Error saving changes!", DrillHoleFile.ExplainError(save) + ".");
                return;
            }

            if (_session != null)
            {
                _session.NotifyFileChanged();
            }
            ResetDataModified();
        }

        private void OnCancel()
        {
            Raise(ScreenId.HoleManagement);
        }

        private void OnBack()
        {
            Raise(ScreenId.HoleManagement);
        }

        // --- Plumbing ---

        private DrillHoleFile HoleFile()
        {
            return _session != null ? _session.HoleFile : null;
        }

        private Control Owner()
        {
            return Content as Control;
        }

        private Window OwnerWindow()
        {
            Control root = Owner();
            return root != null ? Avalonia.Controls.TopLevel.GetTopLevel(root) as Window : null;
        }

        /// <summary>Port of <c>GetSelectedRows</c>: the MDs of the selected rows.</summary>
        private List<decimal> SelectedMDs()
        {
            List<decimal> mds = new List<decimal>();
            foreach (int index in SelectedIndices())
            {
                mds.Add(_absolute.RecordAt(index).MD);
            }
            return mds;
        }

        private void SelectRowAndScrollTo(int index)
        {
            if (_grid == null || index < 0 || index >= _rows.Count)
            {
                return;
            }
            _grid.SelectedIndex = index;
            _grid.ScrollIntoView(_rows[index], null);
        }

        private void SelectLastRow()
        {
            if (_rows.Count > 0)
            {
                SelectRowAndScrollTo(_rows.Count - 1);
            }
        }

        private void Raise(ScreenId destination)
        {
            string path = _session != null ? _session.CurrentFilePath : "";
            EventHandler<NavigateEventArgs> handler = Navigate;
            if (handler != null)
            {
                handler(this, new NavigateEventArgs(destination, path));
            }
        }

        public void OnShown()
        {
            // Legacy Display(): take a private copy so Cancel discards edits.
            ResetDataModified();
            DisableBack();
            _surveyData = new SurveyList(HoleFile().SurveyList);
            UpdateDataGrid();
        }
    }
}
