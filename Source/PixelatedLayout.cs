﻿using System;
using Xamarin.Forms;

namespace VisiPlacement
{
    class PixelatedLayout : LayoutChoice_Set
    {
        public PixelatedLayout(LayoutChoice_Set layoutToManage, double pixelSize)
        {
            this.layoutToManage = layoutToManage;
            pixelWidth = pixelHeight = pixelSize;
            layoutToManage.AddParent(this);
        }

        public override SpecificLayout GetBestLayout(LayoutQuery query)
        {
            query = query.Clone();
            if (this.pixelWidth > 0)
                query.MaxWidth = Math.Floor(query.MaxWidth / this.pixelWidth) * this.pixelWidth;
            if (this.pixelHeight > 0)
                query.MaxHeight = Math.Floor(query.MaxHeight / this.pixelHeight) * this.pixelHeight;
            SpecificLayout internalLayout = this.layoutToManage.GetBestLayout(query);
            if (internalLayout != null) {
                Size size = new Size(Math.Ceiling(internalLayout.Width / this.pixelWidth) * this.pixelWidth, Math.Ceiling(internalLayout.Height / this.pixelHeight) * this.pixelHeight);
                Specific_ContainerLayout result = new Specific_ContainerLayout(null, size, new LayoutScore(), internalLayout, new Thickness(0));
                return this.prepareLayoutForQuery(result, query);
            }
            return null;
        }

        public LayoutChoice_Set LayoutToManage
        {
            get
            {
                return this.layoutToManage;
            }
        }

        LayoutChoice_Set layoutToManage;
        double pixelWidth;
        double pixelHeight;
    }
}
