﻿using System;
using System.Collections.Generic;
using System.Text;
using VisiPlacement;
using Xamarin.Forms;

namespace VisiPlacement
{
    // a PopoutTextbox is a text box that becomes fullscreen while the user edits it
    public class PopoutTextbox : ContainerLayout, OnBack_Listener
    {
        public PopoutTextbox(string title, LayoutStack layoutStack)
        {
            this.layoutStack = layoutStack;

            Button button = new Button();
            button.Clicked += Button_Clicked;
            this.button = button;
            
            ButtonLayout buttonLayout = ButtonLayout.WithoutBevel(button);

            this.textBox = new Editor();
            this.detailsLayout = new TitledControl(title, new TextboxLayout(this.textBox));

            this.SubLayout = new TitledControl(title, buttonLayout);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            this.layoutStack.AddLayout(this.detailsLayout, this);
        }

        public void OnBack(LayoutChoice_Set other)
        {
            this.updateButtonText();
        }

        public string Text
        {
            get
            {
                return this.textBox.Text;
            }
            set
            {
                this.textBox.Text = value;
                this.updateButtonText();
            }
        }

        private void updateButtonText()
        {
            int longestPrefix = 10;
            string text = this.textBox.Text;
            if (text != null && text.Length > longestPrefix)
                this.button.Text = this.textBox.Text.Substring(0, longestPrefix) + "...";
            else
                this.button.Text = text;
        }

        private Button button;
        private Editor textBox;
        private LayoutChoice_Set detailsLayout;
        private LayoutStack layoutStack;
    }
}