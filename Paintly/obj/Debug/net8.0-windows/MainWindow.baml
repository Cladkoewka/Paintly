   M S B A M L   `   `   `  ����B  >Paintly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Paintly.EnumsPaintly.Enums  5   	       Paintly.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=4.6.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   'localclr-namespace:Paintly.Enums  5   	     � Icon$  Resources/icon.ico��5   	    � Title$ Paintly Cladkoewka��5   	   $	��720��6$   $
��1080��61   .��5      �  � RowDefinitions 5	   
   �� 5
      $
��60px �6   �� 5      $��* �6   .+�5   
   c� $��0��6   $��0,0,0,10q�6   
����iA�$��16(�6H   $��Medium"�6V   .@�5      �� -   5         ��Name$ 
SaveButton��5      
�������
�������$
��Save��6T   $��5q�6v   �� 5      -   5        $ PencilButton��5      
�������
�������$��Pencil��6V   $��5q�6�   �� 5      -   5        $ BrushButton��5      
�������
�������$��Brush��6U   $��5q�6}   �� 5      $��Brush type:��6   
�������$��Center=�6?   �� 5      -   5         ��Name$ BrushTypeComboBox��5      
����iA�
�������$��5q�6�   .@�5      �� $��Classic��6      Paintly.Enums.BrushType   Classic
 � Tag# Z 61    �� 
IsSelected$
 True��6Z   �� 5      $��Spray��6      Spray# Z 6/   5      �� 5      -   5        $ RectangleButton��5      
�������
�������$
��Rect��6Y   $��5q�6�   �� 5      -   5        $ CircleButton��5      
�������
�������$��Ellipse��6V   $��5q�6�   �� 5      -   5        $ EraserButton��5      
�������
�������$��Eraser��6V   $��5q�6�   �� 5      $��
Thickness:��6   
�������$��Center=�6=   �� 5      -   5        
�������	 � Minimum$	 1Q�5   (   
 � Maximum$
 20Q�64    � Value$ 1Q�6A   $	��100��6K   $��	10,15,0,0q�6W   5      �� 5       $��Color:��6   $��Center=�6&   �� 5!      -	   5        
���   �
�������$��20��5!   U   $��20��6`   $��5q�6l   �� 5"      -
   5        
�������
�������$��20��5"   U   $��20��6`   $��5q�6l   �� 5#      -   5        
���  ��
�������$��20��5#   Q   $��20��6\   $��5q�6h   �� 5$      -   5        
��� � �
�������$��20��5$   U   $��20��6`   $��5q�6l   �� 5%      -   5        
����  �
�������$��20��5%   S   $��20��6^   $��5q�6j   #  Xceed.Wpf.Toolkit.ColorPicker  5&      -   5        $��40��5&   W   $��20��6b   $��5q�6n   5(      �� 5*   
   -   5        $��drawingCanvas��5*      $��1��6&   
�������5-      5.      5/      