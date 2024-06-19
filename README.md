# KipCart
Используется .Net 7, WPF, EF Core, MySQL 5.7

## Структура БД
![db](https://github.com/TitaniumLexa/KipCart/assets/24435764/7558a5ab-982b-4c4c-89e6-ec611e3fb324)

## Заметки
### Работа с DbContext во Viewmodel
Работа с базой данных не должно являться ответственностью ViewModel. Также это привело к смешению сущности и модели. В ветке *better-mvvm* ситуация несколько лучше, т.к. выделилась модель и сущность бд стала чище.
### Синхронная загрузка данных
Является следствием использования dbContext.\<Entity\>.Local.ToObservableCollection(). Асинхронная загрузка данных приведет к исключения, т.к. будет попытка обновить данные коллекции в UI не из потока объекта Dispatcher. Тогда заполнение коллекции, связанной с UI, нужно проводить в  
```
Application.Current.Dispatcher.Invoke(()=> FillUICollection);
```
