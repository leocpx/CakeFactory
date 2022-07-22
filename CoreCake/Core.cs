using CoreCake.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCake
{
    public class Core
    {
        public static string GetDisplayName(MenuItems item)
        {
            var itemName = item.ToString();
            var enumValue = item.GetType().GetMember(itemName).First();
            var prop = enumValue.GetCustomAttributes(typeof(MenuItemNameAttribute),false).First() as MenuItemNameAttribute;

            return prop.Name;
        }
    }

    public enum MenuItems
    {
        [MenuItemName("SCHEDULES")]
        schedules,
        
        [MenuItemName("PRODUCTION PLANNING")]
        production_planning,

        [MenuItemName("PACKAGING PLANNING")]
        packaging_planning,
        
        [MenuItemName("ACCOUNT ADMINISTRATION")]
        account_administration,
        
        [MenuItemName("INVENTORY MANAGEMENT")]
        inventory_management,
        
        [MenuItemName("DATABASE MANAGEMENT")]
        database_management,
        
        [MenuItemName("REPORTS")]
        reports,
        
        [MenuItemName("SALES")]
        sales,
        
        [MenuItemName("PROGRAM SETTINGS")]
        program_settings,
        
        [MenuItemName("BACK TO MAIN MENU")]
        back_to_mainmenu,
        
        [MenuItemName("PERSONAL LOGIN ACCOUNT")]
        personal_login_account,
        
        [MenuItemName("LOGOUT")]
        logout,
        
        [MenuItemName("CREATE NEW ACCOUNT")]
        create_new_account,
        
        [MenuItemName("MODIFY ACCOUNT")]
        modify_account,
        
        [MenuItemName("DELETE ACCOUNT")]
        delete_account,
        
        [MenuItemName("CLOSE MENU")]
        close_secondMenu,
        
        [MenuItemName("SQL CONNECTION SETTINGS")]
        sql_connection_settings,
        
        [MenuItemName("CREATE RAW GOODS")]
        register_new_raw_goods,
        
        [MenuItemName("CREATE FINISHED GOODS")]
        register_new_finished_goods,
        
        [MenuItemName("MODIFY RAW GOODS")]
        modify_raw_good_info,
        
        [MenuItemName("DELETE RAW GOODS")]
        delete_raw_good_info,
        
        [MenuItemName("MODIFY FINISHED GOODS")]
        modify_finished_good_info,
        
        [MenuItemName("DELETE FINISHED GOODS")]
        delete_finished_good_info
    }
}
