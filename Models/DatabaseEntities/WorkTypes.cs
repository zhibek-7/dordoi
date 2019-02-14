namespace Models.DatabaseEntities
{
    public enum WorkTypes
    {
        Authorize = 1, //1	Авторизация пользователя                                                                            
        Login, //2	Выход пользователя                                                                                  
        CreateProject, //3	Создание проекта                                                                                    
        AddFile, //4	Добавление файла                                                                                    
        UpdateFile, //5	Изменение файла                                                                                     
        AddString, //6	Добавлении строки                                                                                   
        UpdateString, //7	Изменение строки                                                                                    
        DeleteString, //8	Удаление строки                                                                                     
        AddTraslation, //9	Добавление перевода                                                                                 
        DeleteTranslation, //10	Удалении перевода                                                                                   
        UpdateTranslation, //11	Изменение перевода                                                                                  
        ConfirmTranslation, //12	Подтверждение перевода                                                                              
        ChoseTranslation //13	Выбор перевода менеджером
    }
}
