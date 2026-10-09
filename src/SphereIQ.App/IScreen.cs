using System;

namespace SphereIQ.App
{
    /// <summary>
    /// Every workflow screen (welcome, hole planning, reports, ...) implements
    /// this. The shell hosts one at a time in its Content region and wires the
    /// Navigate event to switch screens. Replaces the legacy Procedure_*
    /// + Body_* + FinishedEventArgs stack.
    /// </summary>
    public interface IScreen
    {
        ScreenId Id { get; }

        /// <summary>Root control to place into the shell's content region.</summary>
        object Content { get; }

        /// <summary>Raised when the screen wants to go elsewhere.</summary>
        event EventHandler<NavigateEventArgs> Navigate;

        /// <summary>Called once after the screen is placed into the shell.</summary>
        void OnShown();

        /// <summary>Optional session binding, set by the shell before OnShown.</summary>
        AppSession Session { get; set; }
    }
}
