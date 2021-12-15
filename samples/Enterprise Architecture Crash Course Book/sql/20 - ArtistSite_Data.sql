USE [ArtistSite]
GO
DELETE FROM [dbo].[NewsItem]
GO
DELETE FROM [dbo].[Medium]
GO
DELETE FROM [dbo].[ArtworkMedium_Xref]
GO
DELETE FROM [dbo].[ArtworkCategory_Xref]
GO
DELETE FROM [dbo].[Category]
GO
DELETE FROM [dbo].[Artwork]
GO
DELETE FROM [dbo].[Artist]
GO
SET IDENTITY_INSERT [dbo].[Artist] ON 

INSERT [dbo].[Artist] ([ArtistId], [Name], [Code], [Email], [AboutBlurb], [IsActive], [DisplayOrder], [AppBarImgSrcSmall], [AppBarImgSrcLarge], [HeaderCssClass], [MainContentCssClass], [FooterCssClass]) VALUES (1, N'Gulcin Naggia', N'gulcin-naggia', N'GulcinNaggia@email', N'You can contact Gulcin Naggia at: 
<a class=gulcin-naggia href="mailto:Vicki.Schramm@WhittierArtists.com">
	GulcinNaggia@email
</a><br /><br/>

Links to more of this artist''s work:
<ul>
	<li><a class=gulcin-naggia href="Category/8">All Paintings</a>
</ul>', 1, 60, N'layout/header_tubes.jpg', N'layout/header_tubes.jpg', N'blanched-almond-sandy-beach', N'blanched-almond-sandy-beach', N'footer-tubes')
INSERT [dbo].[Artist] ([ArtistId], [Name], [Code], [Email], [AboutBlurb], [IsActive], [DisplayOrder], [AppBarImgSrcSmall], [AppBarImgSrcLarge], [HeaderCssClass], [MainContentCssClass], [FooterCssClass]) VALUES (2, N'Mor Royston', N'mor-royston', N'mroyston@email', N'<div class=mor-royston>
	You can reach Mor Royston at 
	<a href="mailto:mroyston@email">
		mroyston@email
	</a><br>
	More of Mor''s work can be found here:
	<br>
	<a href="Category/23">
		See All of Mor''s Watercolor Paintings
	</a>
</div>', 1, 40, N'layout/header_tubes.jpg', N'layout/header_tubes.jpg', N'pine-glade-tan', N'pine-glade-tan', N'footer-tubes')
INSERT [dbo].[Artist] ([ArtistId], [Name], [Code], [Email], [AboutBlurb], [IsActive], [DisplayOrder], [AppBarImgSrcSmall], [AppBarImgSrcLarge], [HeaderCssClass], [MainContentCssClass], [FooterCssClass]) VALUES (3, N'Nijole Roma', N'nijole-roma', N'NijoleRoma@email', N'You can contact Nijole Roma at: <a class=nijole-roma href="mailto:NijoleRoma@email">NijoleRoma@email</a>', 1, 20, N'layout/header_tubes.jpg', N'layout/header_tubes.jpg', N'black-cod-gray', N'black-cod-gray', N'footer-tubes')
INSERT [dbo].[Artist] ([ArtistId], [Name], [Code], [Email], [AboutBlurb], [IsActive], [DisplayOrder], [AppBarImgSrcSmall], [AppBarImgSrcLarge], [HeaderCssClass], [MainContentCssClass], [FooterCssClass]) VALUES (4, N'Issoufou Kendall', N'issoufou-kendall', N'issoufou-kendall@email', N'<div>
	Issoufou Kendall can be&nbsp;contacted at: 
	<a class=issoufou-kendall href="mailto:issoufou-kendall@email">
		issoufou-kendall@email
	</a><br>
	View more of Issoufou''s work here:<br>
	<a href="Category/42" class="issoufou-kendall">
		Charitable Work
	</a> ~ 
	<a href="Category/19" class="issoufou-kendall">
		Figurative Work
	</a> ~ 
	<a href="Category/21" class="issoufou-kendall">
		Places Remembered
	</a> ~ 
	<a href="Category/20" class="issoufou-kendall">
		Still Life
	</a> ~ 
	<a href="Category/47" class="issoufou-kendall">
		Jewelry
	</a>
</div>', 1, 50, N'layout/header_pencils.jpg', N'layout/header_pencils.jpg', N'wine-tamarind', N'wine-tamarind', N'footer-pencils')
INSERT [dbo].[Artist] ([ArtistId], [Name], [Code], [Email], [AboutBlurb], [IsActive], [DisplayOrder], [AppBarImgSrcSmall], [AppBarImgSrcLarge], [HeaderCssClass], [MainContentCssClass], [FooterCssClass]) VALUES (5, N'Marvin MacKay', N'marvin-mackay', N'MarvinMacKay@email', N'Marvin MacKay can be reached at 
<a href="mailto:MarvinMacKay@email">
	MarvinMacKay@email
</a><br/>
Look for Marvin on Facebook!<br/>
Find more of Marvin''s work here:<br>
<a href="Category/27">
	Plein Air Paintings
</a>&nbsp;&nbsp;&nbsp;&nbsp;
<a href="Category/28">
	Paintings of Animals
</a>', 1, 70, N'layout/header_tubes.jpg', N'layout/header_tubes.jpg', N'picasso-khaki', N'picasso-khaki', N'footer-tubes')
INSERT [dbo].[Artist] ([ArtistId], [Name], [Code], [Email], [AboutBlurb], [IsActive], [DisplayOrder], [AppBarImgSrcSmall], [AppBarImgSrcLarge], [HeaderCssClass], [MainContentCssClass], [FooterCssClass]) VALUES (6, N'Cali Wechsler', N'cali-wechsler', N'CaliWechsler@email', N'Please contact Cali Wechsler at: <br/><a href="mailto:CaliWechsler@email">CaliWechsler@email</a>', 1, 80, N'layout/header_metal.jpg', N'layout/header_metal.jpg', N'chocolate-ochre', N'chocolate-ochre', N'footer-metal')
INSERT [dbo].[Artist] ([ArtistId], [Name], [Code], [Email], [AboutBlurb], [IsActive], [DisplayOrder], [AppBarImgSrcSmall], [AppBarImgSrcLarge], [HeaderCssClass], [MainContentCssClass], [FooterCssClass]) VALUES (7, N'Amondi Albers', N'amondi-albers', N'AmondiAlbers@email', N'', 1, 10, N'layout/header_camera.jpg', N'layout/header_camera.jpg', N'dark-slate-gray-tuna', N'dark-slate-gray-tuna', N'dark-slate-gray-tuna')
SET IDENTITY_INSERT [dbo].[Artist] OFF
GO
SET IDENTITY_INSERT [dbo].[Artwork] ON 

INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (14, 1, N'Atlantis Hills Dawn', N'For several summer mornings I joined other artist friends for a plein air painting time. The artists work on various projects by providing a good working environment for the crowd and for the people outside the gallery.', 100, NULL, N'gulcin-naggia', N'icon/14.jpg', N'medium/14.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (17, 1, N'Linn’s Valley', N'My husband and I love to visit the town of Cambria on the central coast of California. I know lots of people coming here to enjoy their coffee, play a game or get some dinner.', 100, NULL, N'gulcin-naggia', N'icon/17.jpg', N'medium/17.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (18, 1, N'Greenforest Academy Production of Mice', N'', 100, NULL, N'gulcin-naggia', N'icon/4.jpg', N'medium/4.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (19, 1, N'Shell', N'', 100, NULL, N'gulcin-naggia', N'icon/19.jpg', N'medium/19.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (20, 1, N'Atlantis Hills Sunset', N'', 100, NULL, N'gulcin-naggia', N'icon/20.jpg', N'medium/20.jpg', 1)

INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (84, 2, N'Majestic', N'', 100, NULL, N'mor-royston', N'icon/84.jpg', N'medium/84.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (85, 2, N'Touch Down', N'', 100, NULL, N'mor-royston', N'icon/85.jpg', N'medium/85.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (86, 2, N'Bolsa Chica Wetlands', N'', 100, NULL, N'mor-royston', N'icon/86.jpg', N'medium/86.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (87, 2, N'Yosemite Valley', N'', 100, NULL, N'mor-royston', N'icon/87.jpg', N'medium/87.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (88, 2, N'Among the Aspens', N'', 100, NULL, N'mor-royston', N'icon/88.jpg', N'medium/88.jpg', 1)

INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (96, 3, N'Balboa Pavilion', N'', 100, NULL, N'nijole-roma', N'icon/96.jpg', N'medium/96.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (97, 3, N'The Casino on Catalina Island on a Stormy Day', N'', 100, NULL, N'nijole-roma', N'icon/97.jpg', N'medium/97.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (98, 3, N'Morning on the Arroyo Seco Trail, Pasadena, Ca', N'', 100, NULL, N'nijole-roma', N'icon/98.jpg', N'medium/98.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (99, 3, N'The Grand Canyon, View from the South Rim', N'', 100, NULL, N'nijole-roma', N'icon/99.jpg', N'medium/99.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (100, 3, N'Back Bay, Newport Beach, Ca', N'', 100, NULL, N'nijole-roma', N'icon/100.jpg', N'medium/100.jpg', 1)

INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (190, 4, N'Grandmother''s Things', N'', 100, NULL, N'issoufou-kendall', N'icon/190.gif', N'medium/190.gif', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (191, 4, N'Bittersweet', N'', 100, NULL, N'issoufou-kendall', N'icon/191.jpg', N'medium/191.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (192, 4, N'Red Shoes', N'', 100, NULL, N'issoufou-kendall', N'icon/192.jpg', N'medium/192.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (193, 4, N'Red Hat', N'', 100, NULL, N'issoufou-kendall', N'icon/193.jpg', N'medium/193.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (194, 4, N'Piedras Blancas', N'', 100, NULL, N'issoufou-kendall', N'icon/194.jpg', N'medium/194.jpg', 1)

INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (201, 5, N'Mermaid and Manatee', N'', 100, NULL, N'marvin-mackay', N'icon/201.gif', N'medium/201.gif', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (202, 5, N'Gorilla Mother and Child', N'', 100, NULL, N'marvin-mackay', N'icon/202.gif', N'medium/202.gif', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (203, 5, N'Oyster Creek', N'', 100, NULL, N'marvin-mackay', N'icon/203.jpg', N'medium/203.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (204, 5, N'Descanso Gardens View', N'', 100, NULL, N'marvin-mackay', N'icon/204.jpg', N'medium/204.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (205, 5, N'California Condor', N'', 100, NULL, N'marvin-mackay', N'icon/205.jpg', N'medium/205.jpg', 1)

INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (400, 6, N'Cigarette and La Pistola', N'Pencil on Paper', 100, N'cali-wechsler', N'cali-wechsler', N'icon/400.jpg', N'medium/400.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (401, 6, N'Portrait', N'Wool and cotton thread on Linen', 100, N'cali-wechsler', N'cali-wechsler', N'icon/401.jpg', N'medium/401.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (402, 6, N'After the Rescue', N'Wool and cotton thread on Linen', 100, N'cali-wechsler', N'cali-wechsler', N'icon/402.jpg', N'medium/402.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (403, 6, N'Ancient City (Paul''s Tunisia)', N'Wool and cotton thread on cotton fabric', 100, N'cali-wechsler', N'cali-wechsler', N'icon/403.jpg', N'medium/403.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (404, 6, N'Uncle Ward / Luna / Nannie Rose', N'Pencil on Paper', 100, N'cali-wechsler', N'cali-wechsler', N'icon/404.jpg', N'medium/404.jpg', 1)

INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (642, 7, N'"CC"', N'Leader United Farm Workers…Delano Ca. August 1969', 100, N'amondi-albers', N'amondi-albers', N'icon/642.jpg', N'medium/642.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (643, 7, N'"H.R.C."', N'1992', 100, N'amondi-albers', N'amondi-albers', N'icon/643.jpg', N'medium/643.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (644, 7, N'"I.R.L."', N'Feb. 1973', 100, N'amondi-albers', N'amondi-albers', N'icon/644.jpg', N'medium/644.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (645, 7, N'"P.W.H."', N'circa 1980', 100, N'amondi-albers', N'amondi-albers', N'icon/645.jpg', N'medium/645.jpg', 1)
INSERT [dbo].[Artwork] ([ArtworkId], [ArtistId], [Name], [Description], [DisplayOrder], [LayoutPageName], [CssClass], [IconUri], [ImageUri], [IsActive]) VALUES (646, 7, N'"JB - MN"', N'Hollywood, Golden Globes, 5 March 1962', 100, N'amondi-albers', N'amondi-albers', N'icon/646.jpg', N'medium/646.jpg', 1)

SET IDENTITY_INSERT [dbo].[Artwork] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (1, NULL, 1, N'Gulcin Naggia', N'gulcin-naggia', N'<div align=center>I was fortunate to have both Mor Royston and Nijole Roma as  art  instructors  in college. I grew up with Flanders and Hollandian people. Flanders and Hollandian people taught me English at a very high level, English was the second most learned language of mine, but also French was very slow to write in, but to be honest, I started to learn that German was slower than French. Once I had learnt what I have now, the rest was easy with my French reading.</div>', 2)
INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (8, 1, 1, N'Paintings', N'gulcin-naggia', NULL, 1)

INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (2, NULL, 2, N'Mor Royston', N'mor-royston', N'<div align=center>I have always been interested in all kinds of art and have been drawing and painting for as long as I can remember. I read everything that happened to the great paintings that I''ve ever seen from the paintings of my favorite artists. It never was easy for me to become an artist, and I am grateful to the hundreds of artists, artists around the world who have helped my career rise through the years and who have given their best to my work.</div>', 3)

INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (3, NULL, 3, N'Nijole Roma', N'nijole-roma', N'<div align=center>I have been interested in Art since I was a young boy. I am so blessed to have gained this perspective of the world and so fortunate to have the chance to learn about all aspects of this world. I love to share my love of the world with the students. I look forward to doing more so as soon as I can.</div>', 10)

INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (4, NULL, 4, N'Issoufou Kendall', N'issoufou-kendall', N'<div align=center>My earliest memories of creating are as a young child. With my parents that was the early days of working together to develop community, and the first family was very important to me.</div>', 7)
INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (19, 4, 4, N'Figures and Portraits', N'issoufou-kendall', N'', 1)

INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (5, NULL, 5, N'Marvin MacKay', N'marvin-mackay', N'Marvin has been re-creating animal life and nature in various art mediums for as long as he can remember. Since being given the "Fairyland" project, he''s been doing something that hasn''t been done before, and in this case, one that isn''t a fantasy. He''s also done some of his own illustrations of large animals, and on top of that, he''s been doing a bunch of sketches of small animals that look and feel very different to just some of our favorite animal artwork.', 4)

INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (6, NULL, 6, N'Cali Wechsler', N'cali-wechsler', N'Hey - thanks for visiting my page!  I''m very excited to be joining this fabulous website as a means for displaying my artwork. The website is going to allow you to share a photo with your friends and family so if you have a picture from a trip or vacation, you know the people behind the website need to know. If you have pictures of specific people, your Facebook page will be the place to post them.', 10)

INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (47, 4, 4, N'Jewelry', N'issoufou-kendall', NULL, 1)
INSERT [dbo].[Category] ([CategoryId], [ParentCategoryId], [ArtistId], [Name], [CssClass], [Description], [DisplayOrder]) VALUES (7, NULL, 7, N'Amondi Albers', N'amondi-albers', N'<div align=center>The career of Los Angeles photographer Amondi Albers documents a visual history
that spans over 45 years. The series was a remarkable undertaking and one that drew upon thousands of photographs and footage that was used in a wide range of images and film.
</div>', 0)
SET IDENTITY_INSERT [dbo].[Category] OFF
GO

SET IDENTITY_INSERT [dbo].[Medium] ON 

INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (1, N'Paintings', N'Paintings', 1, 40)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (4, N'Clay', N'Clay', 0, 50)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (5, N'Murals', N'Murals', 1, 130)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (7, N'Photography', N'Photography', 1, 120)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (8, N'Acrylic Paintings', N'Acrylic-Paintings', 1, 30)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (9, N'Watercolor Paintings', N'Watercolor-Paintings', 1, 20)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (10, N'Oil Paintings', N'Oil-Paintings', 1, 10)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (2, N'Pastel', N'Pastel', 1, 100)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (15, N'Jewelry', N'Jewelry', 1, 110)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (16, N'Fiber Arts', N'Fiber-Arts', 1, 70)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (17, N'Graphite Illustrations', N'Graphite-Illustratio', 1, 80)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (4, N'Metal Sculptures', N'Metal-Sculptures', 1, 90)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (19, N'Mixed Media', N'Mixed-Media', 0, 60)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (21, N'Mosaic Glass', N'Mosaic-Glass', 0, 1)
INSERT [dbo].[Medium] ([MediumID], [Name], [Code], [IsActive], [DisplayOrder]) VALUES (5, N'Colored Pencil', N'Colored-Pencil', 0, 140)
SET IDENTITY_INSERT [dbo].[Medium] OFF
GO

SET IDENTITY_INSERT [dbo].[ArtworkMedium_Xref] ON

INSERT INTO [dbo].[ArtworkMedium_Xref]
VALUES
(14,1,1),
(17,1,2),
(18,1,3),
(19,1,4),
(20,1,5),

(84,9,1),
(85,9,2),
(86,9,3),
(87,9,4),
(88,9,5),

(96,10,1),
(97,10,2),
(98,10,3),
(99,10,4),
(100,10,5),

(190,2,1),
(191,2,2),
(192,2,3),
(193,2,4),
(194,2,5),

(201,5,1),
(202,5,2),
(203,5,3),
(204,5,4),
(205,5,5),

(400,16,1),
(401,16,2),
(402,16,3),
(403,16,4),
(404,16,5),

(642,7,1),
(643,7,2),
(644,7,3),
(645,7,4),
(646,7,5)

SET IDENTITY_INSERT [dbo].[ArtworkMediumXref] OFF

SET IDENTITY_INSERT [dbo].[NewsItem] ON 

INSERT [dbo].[NewsItem] ([NewsItemId], [Headline], [NewsCopy]) VALUES (1, N'Mor Royston Student Reunion Art Show 2021', N'Former art students of Mor Royston and the Atlantis Art Gallery invite you to an exhibit in honor of Mor Royston as mentor, teacher and renowned artist. Each month on April 5 at 11pm, your friend, classmate, art teacher, or even your local art gallery will bring their own stories of their own experiences.

Mor Royston Art Center, 2201 S. King St., Room 545')
INSERT [dbo].[NewsItem] ([NewsItemId], [Headline], [NewsCopy]) VALUES (2, N'''The Hunt for Blue December'' featuring Marvin MacKay, July 9 - Aug 31, 2021', N'The City of Atlantis Cultural Arts Commission presents "The Hunt for Blue December," an exhibit featuring paintings by artists who have helped create spectacular artworks.

The museum was created in 2012 after the city closed its doors to the public after it was involved in the investigation of claims found at a church church by police. As well as artifacts, the exhibition was meant to promote city parks on a national scale.')
SET IDENTITY_INSERT [dbo].[NewsItem] OFF
GO
