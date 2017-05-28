﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisiPlacement
{
    public abstract class LayoutQuery
    {
        static int nextID;
        public LayoutQuery()
        {
            this.MinScore = LayoutScore.Minimum;
            this.maxWidth = double.PositiveInfinity;
            this.maxHeight = double.PositiveInfinity;
            this.ID = nextID;
            nextID++;
        }
        // returns whichever layout it likes better
        public abstract LayoutDimensions PreferredLayout(LayoutDimensions choice1, LayoutDimensions choice2);
        // returns whichever layout it likes better
        public SpecificLayout PreferredLayout(SpecificLayout layout1, SpecificLayout layout2)
        {
            if (!this.Accepts(layout1))
            {
                if (this.Accepts(layout2))
                    return layout2;
                else
                    return null;
            }
            if (!this.Accepts(layout2))
            {
                if (this.Accepts(layout1))
                    return layout1;
                else
                    return null;
            }
            LayoutDimensions dimensions1 = new LayoutDimensions();
            dimensions1.Width = layout1.Width;
            dimensions1.Height = layout1.Height;
            dimensions1.Score = layout1.Score;
            LayoutDimensions dimensions2 = new LayoutDimensions();
            dimensions2.Width = layout2.Width;
            dimensions2.Height = layout2.Height;
            dimensions2.Score = layout2.Score;
            if (this.PreferredLayout(dimensions1, dimensions2) == dimensions1)
                return layout1;
            else
                return layout2;
        }
        public abstract LayoutQuery Clone();
        // returns a stricter query given that this example is one of the options
        public abstract void OptimizeUsingExample(SpecificLayout example);
        // returns a stricter query that won't even be satisfied by this example
        public abstract void OptimizePastExample(SpecificLayout example);
        //public abstract void OptimizeUsingExample(LayoutDimensions example);
        public LayoutQuery CopyFrom(LayoutQuery original)
        {
            this.MaxWidth = original.MaxWidth;
            this.MaxHeight = original.MaxHeight;
            this.MinScore = original.MinScore;
            this.Debug = original.Debug;
            this.ProposedSolution_ForDebugging = original.ProposedSolution_ForDebugging;
            return this;
        }
        public bool Accepts(SpecificLayout layout)
        {
            if (layout == null)
                return false;
            if (layout.GetBestLayout(this) != null)
                return true;
            return false;
        }
        public bool Accepts(LayoutDimensions dimensions)
        {
            if (dimensions.Width > this.MaxWidth)
                return false;
            if (dimensions.Height > this.MaxHeight)
                return false;
            if (dimensions.Score.CompareTo(this.MinScore) < 0)
                return false;
            return true;
        }
        
        // whether this query tries to minimize Height
        public virtual bool MinimizesHeight()
        {
            return false;
        }
        public virtual bool MinimizesWidth()
        {
            return false;
        }
        public virtual bool MaximizesScore()
        {
            return false;
        }
        
        public double MaxWidth 
        {
            get
            {
                return this.maxWidth;
            }
            set
            {
                this.maxWidth = value;
                //System.Diagnostics.Debug.WriteLine("max width = ");
                //System.Diagnostics.Debug.WriteLine(this.maxWidth);
            }
        }
        public double MaxHeight
        {
            get
            {
                return this.maxHeight;
            }
            set
            {
                this.maxHeight = value;
                //System.Diagnostics.Debug.WriteLine("max height = ");
                //System.Diagnostics.Debug.WriteLine(this.maxHeight);
            }
        }
        public LayoutScore MinScore
        {
            get
            {
                return this.minScore;
            }
            set
            {
                this.minScore = value;
                //System.Diagnostics.Debug.WriteLine("min score = ");
                //System.Diagnostics.Debug.WriteLine(this.minScore);
            }
        }
        public bool Debug { get; set; } // whether we want to do extra work for this query to ensure the results are correct
        public SpecificLayout ProposedSolution_ForDebugging 
        {
            get
            {
                return this.proposedSolution_forDebugging;
            }
            set
            {
                SpecificLayout proposedSolution = value;
                if ((value != null) && !this.Accepts(proposedSolution))
                {
                    System.Diagnostics.Debug.WriteLine("Error: attempted to provide an invalid debugging solution");
                    // go back and run the original query again
                    LayoutQuery debugQuery = proposedSolution.SourceQuery_ForDebugging;
                    if (debugQuery != null)
                    {
                        debugQuery = debugQuery.Clone();
                        debugQuery.Debug = true;
                        if (proposedSolution.GetAncestors().Count() > 0)
                        {
                            LayoutChoice_Set parent = proposedSolution.GetAncestors().First();
                            SpecificLayout result = parent.GetBestLayout(debugQuery);
                            System.Diagnostics.Debug.WriteLine(result);
                        }
                    }
                }
                this.proposedSolution_forDebugging = proposedSolution;
            }
        }
        public bool Equals(LayoutQuery other)
        {
            LayoutQuery query1 = this;
            LayoutQuery query2 = other;
            if (query1.MaxHeight != query2.MaxHeight)
                return false;
            if (query1.MaxWidth != query2.MaxWidth)
                return false;
            if (query1.MinScore.CompareTo(query2.MinScore) != 0)
                return false;
            if (query1.MinimizesHeight() != query2.MinimizesHeight())
                return false;
            if (query1.MinimizesWidth() != query2.MinimizesWidth())
                return false;
            if (query1.MaximizesScore() != query2.MaximizesScore())
                return false;
            return true;
        }
        private double maxWidth, maxHeight;
        private LayoutScore minScore;
        private SpecificLayout proposedSolution_forDebugging;
        private int ID;
    }
}
