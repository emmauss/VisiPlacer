﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisiPlacement
{
    public class HelpWindowBuilder
    {
        public HelpWindowBuilder()
        {
        }

        public HelpWindowBuilder AddMessage(string message)
        {
            this.messages.AddLast(message);
            return this;
        }

        public LayoutChoice_Set Build()
        {
#if false
            String fullMessage = "";
            foreach (string message in this.messages)
            {
                fullMessage += message + Environment.NewLine + Environment.NewLine;
            }
            TextblockLayout layout = new TextblockLayout(fullMessage);
            return layout;
#else
            GridLayout layout = GridLayout.New(BoundProperty_List.Uniform(this.messages.Count()), BoundProperty_List.Uniform(1), LayoutScore.Zero);
            foreach (string message in this.messages)
            {
                layout.AddLayout(new TextblockLayout(message, 20));
            }
            return layout;
#endif
        }

        private LinkedList<string> messages = new LinkedList<string>();

    }
}
