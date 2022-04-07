using System;
using System.Windows.Forms;
using Assistant.Core;

namespace Assistant.UI
{
    public partial class TextFilterEntry : Form
    {
        private readonly TextFilterEntryModel _entryModel;
        private readonly int _index;
        private readonly bool _isNewEntry;

        public TextFilterEntry() : this(new TextFilterEntryModel(), -1)
        {
            _isNewEntry = true;
        }

        public TextFilterEntry(TextFilterEntryModel entryModel, int index)
        {
            _isNewEntry = false;
            _entryModel = entryModel;
            _index = index;

            InitializeComponent();
            ApplyEntryModelToControls();
        }

        private void ApplyEntryModelToControls()
        {
            filterTextBox.Text = _entryModel.Text;
            checkBoxFilterSysMessages.Checked = _entryModel.FilterSysMessages;
            checkBoxFilterOverhead.Checked = _entryModel.FilterOverhead;
            checkBoxFilterSpeech.Checked = _entryModel.FilterSpeech;
            checkBoxIgnoreFilteredInScripts.Checked = _entryModel.IgnoreFilteredMessageInScripts;

            ok.Enabled = !string.IsNullOrWhiteSpace(filterTextBox.Text);
        }

        private TextFilterEntryModel GetModelFromConfig()
        {
            return new TextFilterEntryModel()
            {
                Text = filterTextBox.Text,
                FilterOverhead = checkBoxFilterOverhead.Checked,
                FilterSpeech = checkBoxFilterSpeech.Checked,
                FilterSysMessages = checkBoxFilterSysMessages.Checked,
                IgnoreFilteredMessageInScripts = checkBoxIgnoreFilteredInScripts.Checked
            };
        }
        
        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            if (_isNewEntry)
            {
                TextFilterManager.AddFilter(GetModelFromConfig());
            }
            else
            {
                TextFilterManager.UpdateFilter(GetModelFromConfig(), _index);
            }

            Close();
        }

        private void filterTextBox_TextChanged(object sender, EventArgs e)
        {
            ok.Enabled = !string.IsNullOrWhiteSpace(filterTextBox.Text);
        }
    }
}
