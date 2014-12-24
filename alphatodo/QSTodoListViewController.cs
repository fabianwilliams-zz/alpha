// This file has been autogenerated from parsing an Objective-C header file added in Xcode.
using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Threading.Tasks;

namespace alphatodo
{
	public partial class QSTodoListViewController : UITableViewController
	{
		private QSTodoService todoService;

		public QSTodoListViewController (IntPtr handle) : base (handle)
		{
		}

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			todoService = QSTodoService.DefaultService;
			await todoService.InitializeStoreAsync ();
							
			RefreshControl.ValueChanged += async (sender, e) => {
				await RefreshAsync ();
			};

			await RefreshAsync ();
		}

		private async Task RefreshAsync ()
		{
			RefreshControl.BeginRefreshing ();
			await todoService.RefreshDataAsync ();
			RefreshControl.EndRefreshing ();

			TableView.ReloadData ();
		}

		#region UITableView methods
		public override int RowsInSection (UITableView tableview, int section)
		{
			if (todoService == null || todoService.Items == null)
				return 0;

			return todoService.Items.Count;
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			const string CellIdentifier = @"Cell";
			var cell = tableView.DequeueReusableCell (CellIdentifier);
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);
			}

			// Set the label on the cell and make sure the label color is black (in case this cell
			// has been reused and was previously greyed out
			var label = (UILabel)cell.ViewWithTag (1);
			label.TextColor = UIColor.Black;
			label.Text = todoService.Items [indexPath.Row].Text;

			return cell;
		}

		public override string TitleForDeleteConfirmation (UITableView tableView, NSIndexPath indexPath)
		{
			// Customize the Delete button to say "complete"
			return @"complete";
		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
		{
			// Find the item that is about to be edited
			var item = todoService.Items [indexPath.Row];

			// If the item is complete, then this is just pending upload. Editing is not allowed
			if (item.Complete)
				return UITableViewCellEditingStyle.None;

			// Otherwise, allow the delete button to appear
			return UITableViewCellEditingStyle.Delete;
		}

		public async override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			// Find item that was commited for editing (completed)
			var item = todoService.Items [indexPath.Row];

			// Change the appearance to look greyed out until we remove the item
			var label = (UILabel)TableView.CellAt (indexPath).ViewWithTag (1);
			label.TextColor = UIColor.Gray;

			// Ask the todoService to set the item's complete value to YES, and remove the row if successful
			await todoService.CompleteItemAsync (item);

			// Remove the row from the UITableView
			tableView.DeleteRows (new [] { indexPath }, UITableViewRowAnimation.Top);
		}

		#endregion

		#region UI Actions
		async partial void OnAdd (UIButton sender)
		{
			if (string.IsNullOrWhiteSpace (itemText.Text))
				return;

			var newItem = new ToDoItem {
				Text = itemText.Text, 
				Complete = false
			};

			await todoService.InsertTodoItemAsync (newItem);

			var index = todoService.Items.FindIndex (item => item.Id == newItem.Id);

			TableView.InsertRows (new [] { NSIndexPath.FromItemSection (index, 0) },
			UITableViewRowAnimation.Top);

			itemText.Text = "";
		}

		#endregion

		[Export ("textFieldShouldReturn:")]
		public virtual bool ShouldReturn (UITextField textField)
		{
			textField.ResignFirstResponder ();
			return true;
		}
	}
}
