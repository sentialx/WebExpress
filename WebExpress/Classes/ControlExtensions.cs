using System.Windows;
using System.Windows.Media;

namespace WebExpress.Controls
{
    public static class ControlExtensions
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            var parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null)
            {
                return null;
            }

            //check if the parent matches the type we're looking for
            var parent = parentObject as T;
            if (parent == null)
            {
                //Parent doesn't match required type, keep looking
                return FindParent<T>(parentObject);
            }
            //Found a match - return it
            return parent;
        }
    }
}