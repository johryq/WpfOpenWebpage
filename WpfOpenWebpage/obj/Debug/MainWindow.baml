   M S B A M L   `   `   `  ����I  EWpfOpenWebpage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WpfOpenWebpageWpfOpenWebpage  5   	       WpfOpenWebpage.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:WpfOpenWebpage     5   	   -   5        $��window��5   	     � Title$  
MainWindow��6   $	��250��6*   $	��400��67    � WindowStyle$
 None=�6C    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�6p   .��5	      �  � RowDefinitions 5
   
   �� 5      $��30 �6   �� 5      .+�5   
   � -   5        
���Gc��.+�5      �� $��OpenWebPage��6   $��Center=�6+   
�������$��10 0 0 0q�6Y   �� 5       �� Orientation$ 
Horizontal=�6   $��Center=�62   $��Right=�6M   $��30��6i   .+�5      �� -   5        $	����5      $��
/#iconfont)�6,   
���   �$��29��6W   $��29��6b   #��[" �6n   $y�Hidden=�6�   �� 5      -   5        $	����5      $��
/#iconfont)�6,   
�������$��29��6W   $��29��6b   #��[" �6n   6�   �� 5      -   5        $	����5      $��
/#iconfont)�6,   
�������$��29��6W   $��29��6b   #��[" �6n   6�   5      5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $	��250 �6#   �� 5      .+�5      � $��5 5 5 5q�6   .+�5       �� -   5         ��Name$ webContentes��5        �� AcceptsReturn . "	 �� VerticalScrollBarVisibility$
	 Auto=�6E   $#��请输入网址,一行一个.��6h   $��15(�6{   5!      � 5"      $��1��6   .+�5#      �� .+�5$      �� -   5        $	��100��5$      $��20��6)   $��0 10 0 0q�65   $��开机自启��6G   6V   �� 5%      -   5        $	��100��5%      $��20��6)   $��0 10 0 0q�65   $��写入服务��6G   $y�Hidden=�6m   �� 5&      -	   5        $	��100��5&      $��20��6)   $��0,10,0,0q�65   $��清除所有配置��6G   6X   5'      5(      5)      5*      5+      