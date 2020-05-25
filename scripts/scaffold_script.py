import sys
import os
os.listdir(".")
sys.path.insert(0, ".")

from ReverseEngineerTables import reverse_engineer_database_tables
reverse_engineer_database_tables.run()