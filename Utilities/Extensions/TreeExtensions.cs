using System;
using System.Collections.Generic;
using System.Linq;
using Models.DatabaseEntities;
using Models.Models;

namespace Utilities.Extensions
{
    public static class TreeExtensions
    {
        public static IEnumerable<TResult> ToTree<TFrom, TResult>(
            this IEnumerable<TFrom> files,
            Func<TFrom, string, TResult> createFactory,
            Func<TFrom, string> iconFactory,
            int? parentId = null
        )
            where TFrom : File
            where TResult : Node<TFrom>, new()
        {
            var nodes = new List<TResult>();

            // Get child nodes by parent node id 
            var childFiles = files.Where(file => file.ID_FolderOwner == parentId)
                .OrderByDescending(file => file.IsFolder)
                .ThenBy(file => file.ID);

            foreach (var file in childFiles)
            {
                var icon = iconFactory(file);

                // Create new node based on file data
                var node = createFactory(file, icon);

                // Get excepted files
                var childItems = files.Except(childFiles).ToTree(createFactory, iconFactory, file.ID);

                // Add child nodes in node
                node.Children.AddRange(childItems);

                // Add node in temporary nodes list
                nodes.Add(node);
            }

            return nodes;
        }
    }
}
