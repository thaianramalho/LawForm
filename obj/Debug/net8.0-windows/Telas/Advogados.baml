   M S B A M L   `   `   `  ����B  >LawForm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:LawFormLawForm  5   	       LawForm.Advogados   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:LawForm     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$  Gerenciamento de Advogados��5	   	   $	��450��6,   $	��800��69    � Icon$ /Telas/logodv.png��6E   .��5
      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $ ��Gerenciamento de Advogados��6   $��Stretch=�5      $	��Top=�6.   $��0��5      $��37��6   $��20(�5      $
��Bold"�6   ���	#FF084F49$��Calibri)�5   '   
�������6<   � 5   
   $��1��6    5      �� 5      $
��Auto �6    �� 5      $��* �6     5      �� 5      $��2* �6#   �� 5       $��3* �6#   .+�5#      �� $��0��6   $��0��6&   $��10,10,10,105q�66   $��2��6L   .+�5$      �� $
��Nome��6   $
��Left=�6'   $	��Top=�6B   �� 5%      -   5         ��Name$ txt_nome��5%      $
��Left=�6,   $	��Top=�6G   $	��200��6_   $��22��6k   �� 5&      $	��CPF��6   $
��Left=�6&   $	��Top=�6A   �� 5'      -   5        $ txt_cpf��5'      $
��Left=�6+   $	��Top=�6F   $	��200��6^   $��22��6j   �� 5(      $��E-mail��6   $
��Left=�6)   $	��Top=�6D   �� 5)      -   5        $ 	txt_email��5)      $
��Left=�6-   $	��Top=�6H   $	��200��6`   $��22��6l   �� 5*      $%��Cadastro Nacional dos Advogados��6   $
��Left=�6B   $	��Top=�6]   �� 5+      -   5        $ txt_cna��5+      $
��Left=�6+   $	��Top=�6F   $	��200��6^   $��22��6j   �� 5,      -   5         ��Name$ 	cadastrar��5,      $��	Cadastrar��6,   $
��Left=�6@   $��0,10,0,0q�6[    �� 	IsDefault$
 true��6�   $��80��6�   �� 5-      -   5        $ 	atualizar��5-      $��	Atualizar��6,   $
��Left=�6@   $��0,10,0,0q�6[   $
 true��6�   $y�	Collapsed=�6�   �� 5.      -   5        $ limpar��5.      $��Limpar��6)   $
 true��6O   $��80��6`   $
��Left=�6k   $��0,10,0,0q�6�   5/      &   System.Windows.Controls.DataGrid  51      -   5          Name$ dataGridAdvogados��51      	   AutoGenerateColumns$	 False��62   $��0��6N   $��1��6[   $��2��6k   $��Stretch=�52      $��Stretch=�6)   $��10q�6E   
   
IsReadOnly$

 True��6Q      CanUserAddRows$ False��54         Columns 55      0  *System.Windows.Controls.DataGridTextColumn  56      ,  &System.Windows.Controls.DataGridColumn   Header$
 Nome��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Nome+6P     57      $	 CPF��6)   $ * 6N    66   �� *Cpf+6N     58      $ E-mail��6)   $ * 6S    69   �� *Email+6S     59      $	 CNA��6)   $ * 6N    66   �� *Cna+6N   4  .System.Windows.Controls.DataGridTemplateColumn  5:      $ Ações��6-      CellTemplate 5;      �� 5<      .R�5=   "   �� -	   5        $��Excluir��5=   )   
 � Tag 6Q   �� *Id+5>      5@      5B      �� 5D      -
   5         ��Name$ logodv��5D      $��10,0,0,5q�6$   $��1��66   $��0��6C   $��70��6S   $��/img/logodv.png��6^   $
��Left=�6w   $��45��6�   $��Bottom=�6�   5E      5F      5G      