using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using EzyPlanData;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Result of <see cref="SurveyEditDialog"/>.
    /// </summary>
    public sealed class SurveyEditResult
    {
        public bool Confirmed { get; set; }

        /// <summary>True when "Add and enter another" was pressed (append/insert loop).</summary>
        public bool AddAnother { get; set; }

        public Survey Survey { get; set; }
    }

    /// <summary>
    /// Port of the legacy <c>Popup_EditSurveyData_Add</c> /
    /// <c>Popup_EditSurveyData_EditSurvey</c> pair. Three fields (MD / Dip /
    /// Azimuth) plus the mode-dependent button set.
    /// <para>
    /// Validation is delegated to <see cref="Survey.SetMD"/>, SetDip and
    /// SetAzimuth so the accepted ranges (MD 0..24000, Dip -90..90 excluding
    /// horizontal, Azimuth 0..360) and their messages stay identical to the
    /// legacy app. The legacy popup validated in order MD, Dip, Azimuth and
    /// reported only the first failure; that order is preserved.
    /// </para>
    /// </summary>
    public static class SurveyEditDialog
    {
        /// <summary>Edit an existing survey in place (legacy "Edit").</summary>
        public static SurveyEditResult Edit(Window owner, Survey survey)
        {
            return Show(owner, survey, "Edit selected survey:", DialogMode.Edit);
        }

        /// <summary>Append a new survey (legacy "Append new survey:").</summary>
        public static SurveyEditResult Append(Window owner)
        {
            return Show(owner, new Survey(), "Append new survey:", DialogMode.Append);
        }

        /// <summary>Insert a new survey above the selected row.</summary>
        public static SurveyEditResult Insert(Window owner)
        {
            return Show(owner, new Survey(), "Insert new survey above selected:", DialogMode.Insert);
        }

        private enum DialogMode
        {
            Edit,
            Append,
            Insert
        }
        private static SurveyEditResult Show(Window owner, Survey survey, string heading, DialogMode mode)
        {
            TextBox txtMd = new TextBox { Width = 180 };
            TextBox txtDip = new TextBox { Width = 180 };
            TextBox txtAzi = new TextBox { Width = 180 };

            // Prefill from the existing survey; blank for new ones.
            if (survey != null)
            {
                txtMd.Text = survey.MD.ToString(CultureInfo.InvariantCulture);
                txtDip.Text = survey.Dip.ToString(CultureInfo.InvariantCulture);
                txtAzi.Text = survey.Azimuth.ToString(CultureInfo.InvariantCulture);
            }

            SurveyEditResult result = new SurveyEditResult();
            TextBlock error = new TextBlock
            {
                Foreground = Brushes.Red,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                IsVisible = false,
                Margin = new Thickness(0, 6, 0, 0)
            };

            StackPanel form = new StackPanel { Margin = new Thickness(20), Spacing = 8 };
            form.Children.Add(new TextBlock { Text = heading, FontSize = 14, TextWrapping = Avalonia.Media.TextWrapping.Wrap });
            Field(form, "MD:", txtMd);
            Field(form, "Dip:", txtDip);
            Field(form, "Azimuth:", txtAzi);
            form.Children.Add(error);

            Window dialog = new Window
            {
                Title = "Edit Survey Data",
                Width = 420,
                SizeToContent = SizeToContent.Height,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = form
            };

            StackPanel buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, HorizontalAlignment = HorizontalAlignment.Right };
            Button cancel = new Button { Content = "Cancel", Width = 90 };

            // Legacy button set: Edit mode had Save only; Add modes had
            // "Add and finish" plus "Add and enter another".
            Button primary;
            if (mode == DialogMode.Edit)
            {
                primary = new Button { Content = "Save", Width = 90 };
            }
            else
            {
                primary = new Button { Content = "Add and finish", Width = 130 };
                Button another = new Button { Content = "Add and enter another", Width = 160 };
                another.Click += (s, e) =>
                {
                    if (Commit(survey, txtMd.Text, txtDip.Text, txtAzi.Text, error))
                    {
                        result.Confirmed = true;
                        result.AddAnother = true;
                        result.Survey = survey;
                        dialog.Close();
                    }
                };
                buttons.Children.Add(another);
            }

            primary.Click += (s, e) =>
            {
                if (Commit(survey, txtMd.Text, txtDip.Text, txtAzi.Text, error))
                {
                    result.Confirmed = true;
                    result.Survey = survey;
                    dialog.Close();
                }
            };
            cancel.Click += (s, e) => dialog.Close();

            buttons.Children.Add(primary);
            buttons.Children.Add(cancel);
            form.Children.Add(buttons);

            if (owner != null)
            {
                dialog.ShowDialog(owner);
            }
            else
            {
                dialog.Show();
            }

            return result;
        }

        private static void Field(Panel parent, string caption, Control editor)
        {
            Grid row = new Grid { Margin = new Thickness(0, 2, 0, 0) };
            row.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            row.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            TextBlock label = new TextBlock { Text = caption, Width = 70, VerticalAlignment = VerticalAlignment.Center };
            row.Children.Add(label);
            Grid.SetColumn(editor, 1);
            row.Children.Add(editor);
            parent.Children.Add(row);
        }

        /// <summary>
        /// Validates MD, then Dip, then Azimuth - the legacy order, so the first
        /// invalid field is the one reported. InvariantCulture is used for parsing
        /// to stay consistent with how values are written back.
        /// </summary>
        private static bool Commit(Survey survey, string mdText, string dipText, string aziText, TextBlock error)
        {
            decimal md;
            if (!decimal.TryParse(mdText, NumberStyles.Any, CultureInfo.InvariantCulture, out md))
            {
                return Fail(error, "Invalid MD - Enter a numeric value.");
            }
            Survey.SetMDEnum mdResult = survey.SetMD(md);
            if (mdResult != Survey.SetMDEnum.ValueSet)
            {
                return Fail(error, Survey.ToHumanReadable(mdResult) + Environment.NewLine + "Enter a valid MD value to continue.");
            }

            decimal dip;
            if (!decimal.TryParse(dipText, NumberStyles.Any, CultureInfo.InvariantCulture, out dip))
            {
                return Fail(error, "Invalid Dip - Enter a numeric value.");
            }
            Survey.SetDipEnum dipResult = survey.SetDip(dip);
            if (dipResult != Survey.SetDipEnum.ValueSet)
            {
                return Fail(error, Survey.ToHumanReadable(dipResult) + Environment.NewLine + "Enter a valid Dip value to continue.");
            }

            decimal azi;
            if (!decimal.TryParse(aziText, NumberStyles.Any, CultureInfo.InvariantCulture, out azi))
            {
                return Fail(error, "Invalid Azimuth - Enter a numeric value.");
            }
            Survey.SetAzimuthEnum aziResult = survey.SetAzimuth(azi);
            if (aziResult != Survey.SetAzimuthEnum.ValueSet)
            {
                return Fail(error, Survey.ToHumanReadable(aziResult) + Environment.NewLine + "Enter a valid Azimuth value to continue.");
            }

            error.IsVisible = false;
            return true;
        }

        private static bool Fail(TextBlock error, string message)
        {
            error.Text = message;
            error.IsVisible = true;
            return false;
        }
    }
}


