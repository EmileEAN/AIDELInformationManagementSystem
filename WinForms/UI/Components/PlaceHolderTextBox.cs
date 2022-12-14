using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.WinForms.UI.Components
{
    public partial class PlaceHolderTextBox : TextBox
    {
        private bool m_isClicked = false;
        private bool m_isPassword = false;

        private string m_placeHolder = "This is a place holder...";
        [
            Category("Appearance"),
            Description("The text to be displayed when the TextBox is empty.")
        ]

        public string PlaceHolder
        {
            get { return m_placeHolder; }
            set { m_placeHolder = value; }
        }

        public PlaceHolderTextBox()
        {
            InitializeComponent();
        }

        public PlaceHolderTextBox(IContainer _container)
        {
            _container.Add(this);

            InitializeComponent();
        }

        public void ActivatePlaceHolderEvents()
        {
            TextChanged += (_sender, _e) =>
            {
                if (m_isClicked)
                    return;

                AddPlaceHolderText();
            };

            Enter += (_sender, _e) =>
            {
                m_isClicked = true;

                PrepareForInput();
            };

            Leave += (_sender, _e) =>
            {
                m_isClicked = false;

                AddPlaceHolderText();
            };

            m_isPassword = UseSystemPasswordChar;
            AddPlaceHolderText();
        }

        private void AddPlaceHolderText()
        {
            if (Text == string.Empty)
            {
                if (m_isPassword)
                    UseSystemPasswordChar = false;

                ForeColor = Color.LightGray;
                Text = m_placeHolder;
            }
        }

        private void PrepareForInput()
        {
            if (m_isPassword)
                UseSystemPasswordChar = true;

            ForeColor = Color.Black;

            if (Text == m_placeHolder)
                Text = string.Empty;
        }
    }
}
