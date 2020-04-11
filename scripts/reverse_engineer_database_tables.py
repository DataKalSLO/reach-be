import json
import os

"""
Background Info: https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding
Reverse Engineering/Scaffolding both mean creating classes from tables in a database. 
Microsoft has luckily provided us a command upon which this process is automated.
This script executes that process for the reach project as well as specifying the tables
to reverse engineer from the file TablesToReverseEngineer.txt
"""

# Constants
overwrite_files = True
path_to_server_root = "../HourglassServer"
file_specifying_tables = "TablesToReverseEngineer.txt"
database_driver = "Npgsql.EntityFrameworkCore.PostgreSQL"
settings_file_name = "appsettings.Development.json"
relative_path_to_output_dir = "Data/Persistent"

# Paths
path_to_settings_file = "/".join([path_to_server_root, settings_file_name])
path_to_file_specifying_tables = "/".join([path_to_server_root, file_specifying_tables])
path_to_output_dir = "/".join([path_to_server_root, relative_path_to_output_dir])  # where classes will be put.
connection_string_key_name = "HourglassDatabase"
json_path_to_settings_file = ["ConnectionStrings",
                              connection_string_key_name]  # series of keys to get to connection string in settings file.
context_file_name = "postgresContext.cs"
path_to_context_file = "/".join([path_to_server_root, relative_path_to_output_dir, context_file_name])

assert os.path.isfile(path_to_file_specifying_tables), "Could not find file containing tables to reverse engineer."
assert os.path.isdir(path_to_output_dir), "Could not find directory at: %s" % path_to_output_dir
assert os.path.isfile(path_to_settings_file), "Could not find settings file at: %s" % path_to_settings_file


def get_connection_string_from_settings_file():
    assert os.path.isfile(path_to_settings_file), "Could not find settings file at %s" % path_to_settings_file
    with open(path_to_settings_file) as f:
        data = json.load(f)
        value = data
    for key in json_path_to_settings_file:
        value = value[key]
    return value


def get_name_of_project_file():
    # Looks for a .csproj file inside of the server root directory.
    query_for_project_file = list(filter(lambda f: ".csproj" in f, os.listdir(path_to_server_root)))
    assert len(query_for_project_file) > 0, "Could not find project file in dir %s" % path_to_server_root
    name_of_project_file = query_for_project_file[0]
    return name_of_project_file


# Constants extended - (placed here because Python has no function hoisting)
app_settings_connection_string = get_connection_string_from_settings_file()
path_to_project_file = "/".join([path_to_server_root, get_name_of_project_file()])
connection_string_replacement = "_config.GetConnectionString(\"%s\")" % connection_string_key_name


def get_command_specifying_tables_to_reverse_engineer():
    """
    Returns the part of the scaffold command that specifies which tables to reverse engineer.
    Note, that when using the CLI tools each table must be specified as a argument to the scaffold command.
    (e.g. --table Story --table DatasetMetaData).
    """
    tables_to_engineer_file = open(path_to_file_specifying_tables)
    tables_to_engineer = tables_to_engineer_file.read()
    tables_to_engineer_file.close()
    lines_in_file = tables_to_engineer.replace("\ufeff", "\n").split("\n")
    table_names = list(map(lambda table_name: table_name.strip(), lines_in_file))
    table_names = list(filter(lambda table_name: len(table_name) > 0 and
                                                 table_name[0].isalpha(),
                              # filters out any systems files or weird chars in text file
                              table_names))
    table_commands = list(map(lambda table_name: "--table \"%s\"" % table_name, table_names))
    return " ".join(table_commands)


def remove_connection_string_from_context_file():
    # Read Context File
    context_file = open(path_to_context_file, "r")
    context_file_content = context_file.read()
    context_file.close()

    # Replace Connection String
    content_without_connection_string = context_file_content.replace("\"%s\"" % app_settings_connection_string,
                                                                     connection_string_replacement)
    context_file = open(path_to_context_file, "w+")
    context_file.write(content_without_connection_string)
    context_file.close()


def remove_connection_string_from_postgres_context():
    """
    This file replaces the connection string inside of postgresContext which a call to extract the connection
    string from the appsettings file. In order to make this replacement a few changes must happen:
    1. Add Configuration package to file. (Microsoft.Extensions.Configuration)
    2. Add configuration to context constructor (valid because dependancy injection settings in startup.cs).
    3. Instantiate a class variable that has access to the configuration
    """
    # Current file identifying strings
    old_constructor_header = "public postgresContext(DbContextOptions<postgresContext> options)"

    # New file strings
    configuration_class_variable = "private IConfiguration _config;"
    new_constructor_header = "public postgresContext(DbContextOptions<postgresContext> options, IConfiguration config)"
    configuration_import_name = "using Microsoft.Extensions.Configuration;\n"
    newline_before_namespace = "\nnamespace"  # above which the import will go.
    config_setting = "\t_config = config;\n\t\t}"

    # Read Content of context file
    context_file = open(path_to_context_file, "r")
    context_file_content = context_file.read()
    context_file.close()

    # Step 1. Write config import
    index_of_config_import = context_file_content.index(newline_before_namespace)
    context_file_content = add_string_at_index(context_file_content, configuration_import_name, index_of_config_import)

    # Step 2. Add dependency injection to constructor for new import
    """
    class variable included here because its easy to replace
    #constructor header with a class variable a top of it.
    """
    new_constructor_with_class_variable = "%s\n\n\t\t%s" % (
        configuration_class_variable, new_constructor_header)
    context_file_content = context_file_content.replace(old_constructor_header, new_constructor_with_class_variable)

    # Step 3. Add instantiation of class variable inside constructor (the new one)
    header_index = context_file_content.index(new_constructor_header)
    end_bracket_index = context_file_content.index("}", header_index)
    context_file_content = add_string_at_index(context_file_content, config_setting, end_bracket_index, offset=1)

    # Step 4. Export changes to file
    context_file = open(path_to_context_file, "w+")  # w+ deletes file content
    context_file.write(context_file_content)
    context_file.close()


def add_string_at_index(base_string, new_string, index, offset=0):
    return base_string[:index] + new_string + base_string[index + offset:]


if __name__ == "__main__":
    base_scaffold_command = "dotnet ef dbcontext scaffold"
    tables_to_reverse_engineer = get_command_specifying_tables_to_reverse_engineer()
    overwrite_files_command = "--force" if overwrite_files else ""

    command = "%s \"%s\" %s --project \"%s\" --output-dir \"%s\" %s %s" % (base_scaffold_command,
                                                                           app_settings_connection_string,
                                                                           database_driver,
                                                                           path_to_project_file,
                                                                           path_to_output_dir,
                                                                           tables_to_reverse_engineer,
                                                                           overwrite_files_command)

    command = command.strip()  # removes extra space if overwrite file command is false

    command_exit_code = os.system(command)

    if command_exit_code != 0:
        raise Exception("Scaffold failed. Received error code: %d" % command_exit_code)

    remove_connection_string_from_context_file()
    remove_connection_string_from_postgres_context()
    print("Finished!")
