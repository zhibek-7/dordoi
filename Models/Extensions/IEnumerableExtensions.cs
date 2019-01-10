using System;
using System.Collections.Generic;
using System.Linq;
using Models.DatabaseEntities;
using Models.Models;

namespace Models.Extensions
{
    public static class IEnumerableExtensions
    {

        public static IEnumerable<TNode> ToTree<TFile, TNode>(
            this IEnumerable<TFile> files,
            Func<TFile, string, TNode> createNodeFromFile,
            Func<TFile, string> getIconForFile,
            int? parentId = null
        )
            where TFile : File
            where TNode : Node<TFile>
        {
            var nodes = new List<TNode>();

            // Get child nodes by parent node id 
            var childFiles = files.Where(file => file.ID_FolderOwner == parentId)
                .OrderByDescending(file => file.IsFolder)
                .ThenBy(file => file.ID);

            foreach (var file in childFiles)
            {
                var icon = getIconForFile(file);

                // Create new node based on file data
                var node = createNodeFromFile(file, icon);

                // Get excepted files
                var childItems = files.Except(childFiles).ToTree(createNodeFromFile, getIconForFile, file.ID);

                // Add child nodes in node
                node.Children.AddRange(childItems);

                // Add node in temporary nodes list
                nodes.Add(node);
            }

            return nodes;
        }

    }
}
