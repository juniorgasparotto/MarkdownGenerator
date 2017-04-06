using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Text;

namespace Html2Markdown.Replacement
{
	internal static class TestHTML
	{
        public static string TestList = @"<ul>
	<li>
		1) DEF
		<ol>
			<li>a</li>
			<li>b</li>
		</ol>
	</li>
	<li>
		2) DEF
		<ol>
			<li>y</li>
			<li>w</li>
		</ol>
	</li>
</ul>

<ol>
	<li>
		A) DEF
		<ul>
			<li>a</li>
			<li>b</li>
		</ul>
	</li>
	<li>
		1
		<ul>
			<li>
				2
				<ul>
					<li>
						3a
						<ul>
							<li>a</li>
							<li>b</li>
						</ul>
					</li>
					<li>3b</li>
				</ul>
			</li>
			<li>w</li>
		</ul>
	</li>
</ol>";
    }
}
