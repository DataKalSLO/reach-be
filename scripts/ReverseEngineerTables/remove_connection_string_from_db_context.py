import os

from ReverseEngineerTables.constants import app_settings_connection_string, path_to_context_file, \
    query_connection_string_from_config_statement, old_constructor_header, \
    constructor_header_with_configuration, \
    namespace_keyword, configuration_import_statement, assign_local_config_to_constructor_config_statement, \
    configuration_class_variable_declaration


def remove_connection_string_from_db_context():
    """
    This file replaces the connection string inside of postgresContext with a call to extract the connection
    string from the appsettings file.

    In order to make this replacement a few changes must happen:
    1. Add Configuration package to file. (Microsoft.Extensions.Configuration)
    2. Add configuration parameter constructor (valid because dependency injection settings in startup.cs).
    3. Add class variable to hold a reference to the IConfiguration of the project
    4. Set the class variable above to the value given in constructor (step 2).
    5. Replace connection string to call for connection string via the configuration class variable
    """
    assert os.path.isfile(path_to_context_file), "No file found in: %s" % path_to_context_file
    context_file_lines = get_file_lines(path_to_context_file)

    # Steps 1, 2, 3, and 4
    create_configuration_class_variable(context_file_lines)

    # Step 5
    connection_string_line_index = get_index_of_line_containing_string(context_file_lines,
                                                                       app_settings_connection_string)
    context_file_lines[connection_string_line_index] = query_connection_string_from_config_statement
    context_file_lines.pop(connection_string_line_index - 1)  # removes warning about having connection string in class.

    # Cleanup: Group lines back to single string representing file
    context_file_content = "\n".join(context_file_lines)
    context_file_content = context_file_content.replace("\t", "    ")  # current project convention uses spaces

    # Step 4. Export changes to file
    context_file = open(path_to_context_file, "w+")  # w+ deletes file content
    context_file.write(context_file_content)
    context_file.close()


def create_configuration_class_variable(context_file_lines):
    # Performs steps 1, 2, 3, and 4 specified in remove_connection_string_from_db_context

    # Step 1. Add configuration import
    namespace_line_index = get_index_of_line_containing_string(context_file_lines, namespace_keyword)
    context_file_lines.insert(namespace_line_index - 2, configuration_import_statement)  # add as last import

    # Step 2. Add configuration parameter to constructor
    constructor_line_index = get_index_of_line_containing_string(context_file_lines, old_constructor_header)
    context_file_lines[constructor_line_index] = "\t\t%s" % constructor_header_with_configuration

    # Step 3. Add class variable for configuration (above constructor)
    constructor_line_index = get_index_of_line_containing_string(context_file_lines,
                                                                 constructor_header_with_configuration)
    context_file_lines.insert(constructor_line_index, configuration_class_variable_declaration)
    context_file_lines.insert(constructor_line_index + 1, "")  # adds new line for formatting

    # Step 3. Set class variable (3) to constructor parameter (2)
    constructor_line_index = get_index_of_line_containing_string(context_file_lines,
                                                                 constructor_header_with_configuration)
    context_file_lines.insert(constructor_line_index + 3,
                              assign_local_config_to_constructor_config_statement)  # skips base call and open bracket


def get_file_lines(path_to_file):
    context_file = open(path_to_file, "r")
    context_file_content = context_file.read()
    context_file.close()
    context_file_lines = context_file_content.split("\n")
    return context_file_lines


def replace_line_containing_string(lines, query_string, replacement_line):
    def update_line(line, query, replacement):
        return line if query not in line else replacement

    return list(map(lambda line: update_line(line, query_string, replacement_line), lines))


def get_index_of_line_containing_string(lines, query):
    for line_index in range(len(lines)):
        if query in lines[line_index]:
            return line_index
    raise Exception("Could not find %s in lines given." % query)
