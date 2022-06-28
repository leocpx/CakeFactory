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
            switch (item)
            {
                case MenuItems.schedules:
                    return "SCHEDULES";
                
                case MenuItems.production_planning:
                    return "PRODUCTION PLANNING"; 
                
                case MenuItems.account_administration:
                    return "ACCOUNT ADMINISTRATION"; 

                case MenuItems.inventory_management:
                    return "INVENTORY MANAGEMENT";

                case MenuItems.reports:
                    return "REPORTS";

                case MenuItems.sales:
                    return "SALES";

                case MenuItems.program_settings:
                    return "PROGRAM SETTINGS";

                case MenuItems.back_to_mainmenu:
                    return "< BACK TO MAIN MENU";

                case MenuItems.logout:
                    return "LOG OUT";

                case MenuItems.create_new_account:
                    return "CREATE NEW ACCOUNT";

                case MenuItems.modify_account:
                    return "MODIFY ACCOUNT";

                case MenuItems.delete_account:
                    return "DELETE ACCOUNT";

                case MenuItems.close_secondMenu:
                    return "< BACK";

                default:
                    return "";
            }
        }
    }

    public enum MenuItems
    {
        schedules,
        production_planning,
        account_administration,
        inventory_management,
        reports,
        sales,
        program_settings,
        back_to_mainmenu,
        personal_login_account,
        logout,
        create_new_account,
        modify_account,
        delete_account,
        close_secondMenu
    }
}
