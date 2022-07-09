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
                case MenuItems.worker_production_orders:
                    return "PRODUCTION ORDERS";

                case MenuItems.worker_packaging_orders:
                    return "PACKAGING ORDERS";

                case MenuItems.admin_production_planning:
                    return "PRODUCTION PLANNING";

                case MenuItems.admin_packaging_planning:
                    return "PACKAGING PLANNING";

                case MenuItems.account_administration:
                    return "ACCOUNT ADMINISTRATION"; 

                case MenuItems.inventory_management:
                    return "INVENTORY MANAGEMENT";

                case MenuItems.database_management:
                    return "DATABASE MANAGEMENT";

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

                case MenuItems.sql_connection_settings:
                    return "SQL CONNECTION SETTINGS";

                case MenuItems.register_new_raw_goods:
                    return "CREATE RAW GOODS";

                case MenuItems.register_new_finished_goods:
                    return "CREATE FINISHED GOODS";

                case MenuItems.modify_raw_good_info:
                    return "MODIFY RAW GOOD";

                case MenuItems.modify_finished_good_info:
                    return "MODIFY FINISHED GOOD";

                case MenuItems.delete_raw_good_info:
                    return "DELETE RAW GOOD";

                case MenuItems.delete_finished_good_info:
                    return "DELETE FINISHED GOOD";

                default:
                    return "";
            }
        }
    }

    public enum MenuItems
    {
        worker_production_orders,
        worker_packaging_orders,
        admin_production_planning,
        admin_packaging_planning,
        account_administration,
        inventory_management,
        database_management,
        reports,
        sales,
        program_settings,
        back_to_mainmenu,
        personal_login_account,
        logout,
        create_new_account,
        modify_account,
        delete_account,
        close_secondMenu,
        sql_connection_settings,
        register_new_raw_goods,
        register_new_finished_goods,
        modify_raw_good_info,
        delete_raw_good_info,
        modify_finished_good_info,
        delete_finished_good_info
    }
}
