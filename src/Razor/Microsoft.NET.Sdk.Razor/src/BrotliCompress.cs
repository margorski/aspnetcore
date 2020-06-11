// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using Microsoft.Build.Framework;

namespace Microsoft.AspNetCore.Razor.Tasks
{
    public class BrotliCompress : DotNetToolTask
    {
        [Required]
        public ITaskItem[] FilesToCompress { get; set; }

        public string CompressionLevel { get; set; }

        internal override string Command => "brotli";

        protected override string GenerateResponseFileCommands()
        {
            var builder = new StringBuilder();

            builder.AppendLine(Command);

            if (!string.IsNullOrEmpty(CompressionLevel))
            {
                builder.AppendLine("-c");
                builder.AppendLine(CompressionLevel);
            }

            for (var i = 0; i < FilesToCompress.Length; i++)
            {
                var input = FilesToCompress[i];
                var source = input.GetMetadata("FullPath");
                var output = input.GetMetadata("TargetPath");

                if (File.Exists(source) && File.GetLastWriteTimeUtc(source) < File.GetLastWriteTimeUtc(output))
                {
                    // Incrementalism. If input source doesn't exist or it exists and is not newer than the expected output, do nothing.
                    Log.LogMessage(MessageImportance.Low, $"Skipping '{source}' because '{output}' is newer than '{source}'.");
                    continue;
                }

                builder.AppendLine("-s");
                builder.AppendLine(source);

                builder.AppendLine("-o");
                builder.AppendLine(output);
            }

            return builder.ToString();
        }
    }
}
