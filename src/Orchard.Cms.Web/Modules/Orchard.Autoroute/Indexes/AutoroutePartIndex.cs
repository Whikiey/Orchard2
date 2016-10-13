﻿using System;
using Orchard.Autoroute.Model;
using YesSql.Core.Indexes;

namespace Orchard.ContentManagement.Records
{
    public class AutoroutePartIndex : MapIndex
    {
        public int ContentItemId { get; set; }
        public string Path { get; set; }
    }

    public class AutoroutePartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<AutoroutePartIndex>()
                .Map(contentItem =>
                {
                    var path = contentItem.As<AutoroutePart>()?.Path;
                    if (!String.IsNullOrEmpty(path))
                    {
                        return new AutoroutePartIndex
                        {
                            ContentItemId = contentItem.ContentItemId,
                            Path = path
                        };
                    }

                    return null;
                });
        }
    }
}