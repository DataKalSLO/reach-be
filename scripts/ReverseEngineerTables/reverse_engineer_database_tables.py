import os

from constants import context_file_name, path_to_table_to_generate_file, \
    path_to_models_output_dir, path_to_app_settings, database_driver, overwrite_files, \
    output_dir_name, path_to_context_dir, app_settings_connection_string, path_to_project_file, \
    context_dir_name
from remove_connection_string_from_db_context import remove_connection_string_from_db_context

"""
Background Info: https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding
Reverse Engineering/Scaffolding both mean creating classes from tables in a database. 
Microsoft has luckily provided us a command upon which this process is automated.
This script executes that process for the reach project as well as specifying the tables
to reverse engineer from the file TablesToReverseEngineer.txt
"""

assert os.path.isfile(path_to_table_to_generate_file), "Could not find file containing tables to reverse engineer."
assert os.path.isdir(path_to_models_output_dir), "Could not find directory at: %s" % path_to_models_output_dir
assert os.path.isfile(path_to_app_settings), "Could not find settings file at: %s" % path_to_app_settings


def get_command_specifying_tables_to_reverse_engineer():
    """
    Returns the part of the scaffold command that specifies which tables to reverse engineer.
    Note, that when using the CLI tools each table must be specified as a argument to the scaffold command.
    (e.g. --table Story --table DatasetMetaData).
    """
    tables_to_engineer_file = open(path_to_table_to_generate_file)
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


def run():
    base_scaffold_command = "dotnet ef dbcontext scaffold"
    tables_to_reverse_engineer = get_command_specifying_tables_to_reverse_engineer()
    overwrite_files_command = "--force" if overwrite_files else ""
    print(path_to_context_dir)
    command = "%s \"%s\" %s --context %s --context-dir %s --project \"%s\" --output-dir \"%s\" %s %s" % (
        base_scaffold_command,
        app_settings_connection_string,
        database_driver,
        context_file_name,
        context_dir_name,
        path_to_project_file,
        output_dir_name,
        tables_to_reverse_engineer,
        overwrite_files_command)
    print(command)
    command = command.strip()  # removes extra space if overwrite file command is false
    command_exit_code = os.system(command)

    if command_exit_code != 0:
        raise Exception("Scaffold failed. Received error code: %d" % command_exit_code)

    remove_connection_string_from_db_context()
    print("Finished!")
