using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Petzold.DumpControlTemplate
{
    public class ControlMenuItem : MenuItem
    {
        public ControlMenuItem()
        {
            // Создаем сборку 
            Assembly asbly = Assembly.GetAssembly(typeof(Control));

            // Массив типов
            Type[] atype = asbly.GetTypes();

            // Мы собираемся сохранить потомков Control в отсортированном списке.
            SortedList<string, MenuItem> sortlst = 
                                    new SortedList<string, MenuItem>();

            Header = "Control";
            Tag = typeof(Control);
            sortlst.Add("Control", this);

            // Перечисляем все типы в массиве.Добавляем сорт лист
            foreach (Type typ in atype)
            {
                if (typ.IsPublic && (typ.IsSubclassOf(typeof(Control))))
                {
                    MenuItem item = new MenuItem();
                    item.Header = typ.Name;
                    item.Tag = typ;
                    sortlst.Add(typ.Name, item);
                }
            }

            // проходим сисок. Устанавливаем родителей
            foreach (KeyValuePair<string, MenuItem> kvp in sortlst)
            {
                if (kvp.Key != "Control")
                {
                    string strParent = ((Type)kvp.Value.Tag).BaseType.Name;
                    MenuItem itemParent = sortlst[strParent];
                    itemParent.Items.Add(kvp.Value);
                }
            }

            // Сканирование еще раз
            
            foreach (KeyValuePair<string, MenuItem> kvp in sortlst)
            {
                Type typ = (Type)kvp.Value.Tag;

                if (typ.IsAbstract && kvp.Value.Items.Count == 0)
                    kvp.Value.IsEnabled = false;

                if (!typ.IsAbstract && kvp.Value.Items.Count > 0)
                {
                    MenuItem item = new MenuItem();
                    item.Header = kvp.Value.Header as string;
                    item.Tag = typ;
                    kvp.Value.Items.Insert(0, item);
                }
            }
        }
    }
}
