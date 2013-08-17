TdsProjectCleaner
=================

Merge conflicts in TDS can be a bit of a nightmare to clean up if there have been significant changes on both sides.  In some cases, the best thing to do is to normalise both versions to make the merge easier.  That's what this utility aims to do.

First of all, in a TDS project file, the `<Icon>` element can be represented in two ways and TDS, at one point, would flip-flop between the two.  This caused false conflicts to occur and generally made file comparison a pain.  This tool will convert them all to one format.

Second, as the project goes along, and as merges occur, the order of items in the project file becomes slightly fragmented.  This tool can sort them all back in to order.

Finally, it can remove items that are present in the project file but, for one reason or another reference a file that no longer exists on the disk (or vice versa).  Typically this is down to poor merge resolution, or mistakes during commit; but it's something that TDS can occasionally throw a wobbler over.

