﻿using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Flow.Launcher.Infrastructure.Logger;
using Flow.Launcher.ViewModel;

namespace Flow.Launcher.Converters
{
    public class QuerySuggestionBoxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3)
            {
                return string.Empty;
            }
            var QueryText = values[1] as TextBox;
            var Suggestion = values[0] as TextBox;
            Suggestion.ScrollToHorizontalOffset(QueryText.HorizontalOffset);
            // first prop is the current query string
            var queryText = QueryText.Text;

            if (string.IsNullOrEmpty(queryText))
                return string.Empty;

            // second prop is the current selected item result
            var val = values[2];
            if (val == null)
            {
                return string.Empty;
            }
            if (!(val is ResultViewModel))
            {
                return System.Windows.Data.Binding.DoNothing;
            }

            try
            {
                var selectedItem = (ResultViewModel)val;

                var selectedResult = selectedItem.Result;
                var selectedResultActionKeyword = string.IsNullOrEmpty(selectedResult.ActionKeywordAssigned) ? "" : selectedResult.ActionKeywordAssigned + " ";
                var selectedResultPossibleSuggestion = selectedResultActionKeyword + selectedResult.Title;

                if (!selectedResultPossibleSuggestion.StartsWith(queryText, StringComparison.CurrentCultureIgnoreCase))
                    return string.Empty;

                // When user typed lower case and result title is uppercase, we still want to display suggestion
                return queryText + selectedResultPossibleSuggestion.Substring(queryText.Length);
            }
            catch (Exception e)
            {
                Log.Exception(nameof(QuerySuggestionBoxConverter), "fail to convert text for suggestion box", e);
                return string.Empty;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}