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
            InitializeComponent();
            _isNewEntry = true;
        }

        public TextFilterEntry(TextFilterEntryModel entryModel, int index)
        {
            _isNewEntry = false;
            _entryModel = entryModel;
            _index = index;

            ApplyEntryModelToControls();
        }

        private void ApplyEntryModelToControls()
        {
            filterTextBox.Text = _entryModel.Text;
            checkBoxFilterSysMessages.Checked = _entryModel.FilterSysMessages;
            checkBoxFilterOverhead.Checked = _entryModel.FilterOverhead;
            checkBoxFilterSpeech.Checked = _entryModel.FilterSpeech;
            checkBoxIgnoreFilteredInScripts.Checked = _entryModel.IgnoreFilteredMessageInScripts;
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void cancel_Click(object sender, EventArgs e)
        {
        }

        private void ok_Click(object sender, EventArgs e)
        {
        }
    }
}
