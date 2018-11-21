1.	ПРОЕКТ ЛОКАЛИЗАЦИИ ПРИЛОЖЕНИЙ

1.1.	Общие положения по проекту
1)	Название проектов, объектов  и так далее обсуждается.
2)	Расположение объектов тоже обсуждается.

1.2.	Структура проекта
3)	Localization – основное приложение.
4)	Всю бизнес логику C# выносить в отдельный проект Models. В последствии будем писать мобильное приложение, которое будет использовать туже бизнес логику.
5)	Утилиты работы с чем-то лучше выносить в Utilities, возможно оптом их будем использовать и для других приложений.
6)	Скрипты работы с базой данных выносить в DAL.
?	Предлогаю использовать Npgsql EF Core provider — Npgsql.EntityFrameworkCore.PostgreSQL (обсуждаеться).
?	нельзя использовать Entity Framework. Пишут что у Entity Framework проблемы с быстродействием на больших проектов. 
?	ODBC – нельзя использовать, т.к. как заказчик потом хочет все на linux серверах использовать. 


1.3.	Проверка работы проекта
1)	Нужно откомлилять в VS или запустить ангуляр приложение и открыть страницу http://localhost:8880/work-panel (порт может быть любой). Результат на картинке ниже.
 
2)	Сейчас работа над базовой папкой проекта

1.4.	Описание проекта на Agular

1.4.1.	IDE

1)	Для работы с Angular можно использовать Visual Studio Code. Проект для открытия .\Localization\client.code-workspace.

1.4.2.	Описание структуры папок
1)	Исходники находятся в папке . Localization\ClientApp\src\app
2)	В проекте используется ленивая загрузка, то есть страницы загружаются только при обращении (как сделать будет описано ниже).
3)	\Localization\ClientApp\src\app\app.module.ts – базовый модуль, туда подключать нельзя. Он подключает  базовый модулю CoreModule .
4)	CoreModule – главный модуль в котором подключаются все остальные модули.
5)	Вспомогательные модули, библиотеки, тестовые далее и так далее лежат в в подпаках \Localization\ClientApp\src\app\: admin, entities, models, services,  work-panel. В эту подпапку добавляйте свои разработки.
6)	В core.module.ts – подключаете только необходимые вам import.
7)	В core-routing.model.ts содержится объявление Routes – в рамках всей программы. Если вам нужно сделать навигацию в рамках свой странице можно посмотреть на admin-routing.module.ts. 
8)	Навигация и ленивая загрузка описана в разделе 1.1.3.

1.4.3.	Навигация и ленивая загрузка
1)	Ленивая загрузка позволяет не загружать все js приложение за раз, а загружать только если пользователь просить открыть страницу. Для ее реализации необходимо разбить приложение на модули, которые и будут подгружаться при необходимости.
2)	Для использования ленивой загрузки необходимо использовать ключевое слово loadChildren.
3)	В core-routing.model.ts содержится объявление Routes – в рамках всей программы.  Пример:
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

4)	Строка path: 'work-panel' говорить системе по какому url страница будет доступна. Пример url http://localhost:8880/work-panel
5)	Строка '../work-panel/work-panel.module#WorkPanelModule' – показывать место расположения модуля.
6)	Ключевое слово loadChildren говорит системе, что не нужно загружать сразу данный модуль.

1.4.4.	Полезные скрипты запуска
?	\Localization\ClientApp\__build_prod.bat – собирает js для отправки на сервер приложений
?	\Localization\ClientApp\0_install_node_modules.bat – устанавливает библиотеки из  node_modules
?	\Localization\ClientApp\1_NPM setup.bat – обновляет библиотеки npm и node_modules. 
?	\Localization\ClientApp\10_start.bat – запускает приложение ангуляр.
?	\Localization\ClientApp\4_node_version.bat – для проверки вырсии node и npm
?	\Localization\ClientApp\7_open_url.bat – открывает урл http://localhost:4200 в браузере

1.5.	Библиотеки Visual Studio Code
1)	Библиотеки, которые я использую в Visual Studio Code (не уверен, что они вам все нужны)

1.6.	Полезные ссылки
1)	Введение в модули Angular — корневой модуль (Root Module)
https://habr.com/post/351504/
2)	Пример создания приложения SINGLE PAGE APPLICATION : ASP.NET MVC .NET CORE + ANGULAR
 https://www.siv-blog.com/single-page-application-asp-net-mvc-net-core-angular-4-part-1/ - немного устарело
3)	Angular Elements - How to use in ASP.NET Core using Angular CLI?
https://www.mithunvp.com/using-angular-elements-asp-net-core-angular-cli-visual-studio/
4)	https://docs.microsoft.com/ru-ru/aspnet/core/client-side/spa-services?view=aspnetcore-2.1 Использование JavaScriptServices для создания одностраничных приложений ASP.NET Core (не знаю как переключить на оригинальный текст(английский)).
5)	Ленивая загрузка: разделение кода NgModules с Webpack https://webformyself.com/lenivaya-zagruzka-razdelenie-koda-ngmodules-s-webpack/

 
 
 
 
 


?

