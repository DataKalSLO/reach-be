import json
import os

# Command Constants
overwrite_files = True
database_driver = "Npgsql.EntityFrameworkCore.PostgreSQL"
tables_to_generate_file_name = "TablesToReverseEngineer.txt"
app_settings_file_name = "appsettings.Development.json"
output_dir_name = os.path.join("Models", "Persistent")
context_file_name = "HourglassContext"
connection_string_key_name = "HourglassDatabase"

# DbContext constants
namespace_keyword = "namespace"
configuration_import_statement = "using Microsoft.Extensions.Configuration;"
configuration_class_variable_declaration = "\t\tprivate readonly IConfiguration _config;"
assign_local_config_to_constructor_config_statement = "\t\t\t_config = config;"
old_constructor_header = "public %s(DbContextOptions<%s> options)" % (context_file_name, context_file_name)
constructor_header_with_configuration = "public %s(DbContextOptions<%s> options, IConfiguration config)" % (
    context_file_name, context_file_name)

# Paths
path_to_project_folder = os.path.join("..", "..", "HourglassServer")
path_to_app_settings = os.path.join(path_to_project_folder, app_settings_file_name)
path_to_table_to_generate_file = os.path.join(path_to_project_folder, tables_to_generate_file_name)
path_to_models_output_dir = os.path.join(
    path_to_project_folder, output_dir_name)  # where classes will be put.
json_path_to_settings_file = ["ConnectionStrings",  # series of keys to get to connection string in settings file.
                              connection_string_key_name]
query_connection_string_from_config_statement = "\t\t\t\toptionsBuilder.UseNpgsql(_config.GetConnectionString(\"%s\"));" % \
                                                connection_string_key_name
context_dir_name = "Data"
path_to_context_dir = os.path.join(path_to_project_folder, context_dir_name)
path_to_context_file = os.path.join(path_to_context_dir, context_file_name + ".cs")


def get_connection_string_from_app_settings():
    assert os.path.isfile(path_to_app_settings), "Could not find settings file at %s" % path_to_app_settings
    with open(path_to_app_settings) as f:
        data = json.load(f)
        value = data
    for key in json_path_to_settings_file:
        value = value[key]
    return value


def get_project_file_name():
    # Looks for a .csproj file inside of the server root directory.
    query_for_project_file = list(filter(lambda f: ".csproj" in f, os.listdir(path_to_project_folder)))
    assert len(query_for_project_file) > 0, "Could not find project file in dir %s" % path_to_project_folder
    name_of_project_file = query_for_project_file[0]
    return name_of_project_file


# Constants extended - (placed here because Python has no function hoisting)
path_to_project_file = os.path.join(path_to_project_folder, get_project_file_name())
app_settings_connection_string = get_connection_string_from_app_settings()
