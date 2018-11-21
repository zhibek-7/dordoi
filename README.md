1.	������ ����������� ����������

1.1.	����� ��������� �� �������
1)	�������� ��������, ��������  � ��� ����� �����������.
2)	������������ �������� ���� �����������.

1.2.	��������� �������
3)	Localization � �������� ����������.
4)	��� ������ ������ C# �������� � ��������� ������ Models. � ����������� ����� ������ ��������� ����������, ������� ����� ������������ ���� ������ ������.
5)	������� ������ � ���-�� ����� �������� � Utilities, �������� ����� �� ����� ������������ � ��� ������ ����������.
6)	������� ������ � ����� ������ �������� � DAL.
?	��������� ������������ Npgsql EF Core provider � Npgsql.EntityFrameworkCore.PostgreSQL (������������).
?	������ ������������ Entity Framework. ����� ��� � Entity Framework �������� � ��������������� �� ������� ��������. 
?	ODBC � ������ ������������, �.�. ��� �������� ����� ����� ��� �� linux �������� ������������. 


1.3.	�������� ������ �������
1)	����� ����������� � VS ��� ��������� ������� ���������� � ������� �������� http://localhost:8880/work-panel (���� ����� ���� �����). ��������� �� �������� ����.
 
2)	������ ������ ��� ������� ������ �������

1.4.	�������� ������� �� Agular

1.4.1.	IDE

1)	��� ������ � Angular ����� ������������ Visual Studio Code. ������ ��� �������� .\Localization\client.code-workspace.

1.4.2.	�������� ��������� �����
1)	��������� ��������� � ����� . Localization\ClientApp\src\app
2)	� ������� ������������ ������� ��������, �� ���� �������� ����������� ������ ��� ��������� (��� ������� ����� ������� ����).
3)	\Localization\ClientApp\src\app\app.module.ts � ������� ������, ���� ���������� ������. �� ����������  ������� ������ CoreModule .
4)	CoreModule � ������� ������ � ������� ������������ ��� ��������� ������.
5)	��������������� ������, ����������, �������� ����� � ��� ����� ����� � � �������� \Localization\ClientApp\src\app\: admin, entities, models, services,  work-panel. � ��� �������� ���������� ���� ����������.
6)	� core.module.ts � ����������� ������ ����������� ��� import.
7)	� core-routing.model.ts ���������� ���������� Routes � � ������ ���� ���������. ���� ��� ����� ������� ��������� � ������ ���� �������� ����� ���������� �� admin-routing.module.ts. 
8)	��������� � ������� �������� ������� � ������� 1.1.3.

1.4.3.	��������� � ������� ��������
1)	������� �������� ��������� �� ��������� ��� js ���������� �� ���, � ��������� ������ ���� ������������ ������� ������� ��������. ��� �� ���������� ���������� ������� ���������� �� ������, ������� � ����� ������������ ��� �������������.
2)	��� ������������� ������� �������� ���������� ������������ �������� ����� loadChildren.
3)	� core-routing.model.ts ���������� ���������� Routes � � ������ ���� ���������.  ������:
const routes: Routes = [
    {
        path: 'work-panel',
        loadChildren: '../work-panel/work-panel.module#WorkPanelModule'
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'admin',
        loadChildren: '../admin/admin.model#AdminModule'
    },
    {
        path: '**',
        component: NotFoundComponent
    }
];

4)	������ path: 'work-panel' �������� ������� �� ������ url �������� ����� ��������. ������ url http://localhost:8880/work-panel
5)	������ '../work-panel/work-panel.module#WorkPanelModule' � ���������� ����� ������������ ������.
6)	�������� ����� loadChildren ������� �������, ��� �� ����� ��������� ����� ������ ������.

1.4.4.	�������� ������� �������
?	\Localization\ClientApp\__build_prod.bat � �������� js ��� �������� �� ������ ����������
?	\Localization\ClientApp\0_install_node_modules.bat � ������������� ���������� ��  node_modules
?	\Localization\ClientApp\1_NPM setup.bat � ��������� ���������� npm � node_modules. 
?	\Localization\ClientApp\10_start.bat � ��������� ���������� �������.
?	\Localization\ClientApp\4_node_version.bat � ��� �������� ������ node � npm
?	\Localization\ClientApp\7_open_url.bat � ��������� ��� http://localhost:4200 � ��������

1.5.	���������� Visual Studio Code
1)	����������, ������� � ��������� � Visual Studio Code (�� ������, ��� ��� ��� ��� �����)

1.6.	�������� ������
1)	�������� � ������ Angular � �������� ������ (Root Module)
https://habr.com/post/351504/
2)	������ �������� ���������� SINGLE PAGE APPLICATION : ASP.NET MVC .NET CORE + ANGULAR
 https://www.siv-blog.com/single-page-application-asp-net-mvc-net-core-angular-4-part-1/ - ������� ��������
3)	Angular Elements - How to use in ASP.NET Core using Angular CLI?
https://www.mithunvp.com/using-angular-elements-asp-net-core-angular-cli-visual-studio/
4)	https://docs.microsoft.com/ru-ru/aspnet/core/client-side/spa-services?view=aspnetcore-2.1 ������������� JavaScriptServices ��� �������� �������������� ���������� ASP.NET Core (�� ���� ��� ����������� �� ������������ �����(����������)).
5)	������� ��������: ���������� ���� NgModules � Webpack https://webformyself.com/lenivaya-zagruzka-razdelenie-koda-ngmodules-s-webpack/

 
 
 
 
 


?

