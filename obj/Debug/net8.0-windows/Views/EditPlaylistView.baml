   M S B A M L   `   `   `  ����I  ELabb2-Database, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Labb2_Database.ViewModelLabb2_Database.ViewModel  5   	   /    )Labb2_Database.ViewModel.EditPlaylistView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   8local&clr-namespace:Labb2_Database.ViewModel     5   	     � Title$  EditPlaylistViewModel��5	   	   $	��450��6'   $	��800��64    � DataContext 5   	   �� *EditPlaylistViewModel+.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��20 0 0 0q�6   ��6"   �� *currentPlaylistName+&   System.Windows.Controls.DataGrid  5   
      AutoGenerateColumns$ False��5         CanUserAddRows$ False��5         CanUserResizeColumns$ False��5         CanUserDeleteRows$ False��5         
IsReadOnly$
 True��5      	   SelectionMode$	 Single=�5      $��
20 0 20 20q�5       $��1��5!      ��5      �� *TracksInPlaylist+
 �� SelectedItem
 5      �� *SelectedTrackInPlaylist+   Columns 5"      0  *System.Windows.Controls.DataGridTextColumn  5#      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Name+  5$      $ Composer��6%    67   �� *
Composer+  5%      $ Milliseconds��6%    6;   �� *Milliseconds+5'      �� 5)   
   $��20 0 0 0q�6   $��1��6"   $��Track Library��62     5*   
   $ False��5-      $ False��5.      $ False��5/      $ False��50      $
 True��51      $	 Single=�52      $��1��53      $��1��54      $��
20 0 20 20q�55      ��5+      �� *AvailableTracks+
 5,      �� *SelectedTrackInTracks+ 56        57      $
 Name��6%    63   �� *Name+  58      $ Composer��6%    67   �� *
Composer+  59      $ Milliseconds��6%    6;   �� *Milliseconds+5;      � 5>   
   $��1��6   $��2��6   .+�5?      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   .+�5@      �� $��20��5B      $	��150��5C      $��	20 0 0 20q�5D      ��5A      �� *AddTrackToPlaylistCommand+.��5D   (   Add Track To Playlist5F      �� 5G      $��20��5I      $	��150��5J      $��	20 0 0 20q�5K      ��5H      �� * RemoveTrackFromPlaylistCommand+.��5K   (   Remove Track From Playlist5M      5N      5O      5P      5Q      