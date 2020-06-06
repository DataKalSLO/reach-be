import pandas as pd
import json
import psycopg2

def handler(event, context):
    DATA_URL = 'http://files.zillowstatic.com/research/public/Zip/Sale_Prices_Zip.csv'
    df = pd.read_csv(DATA_URL)

    # Get all zips we're interested in
    with open("zips.json") as f:
        json_zips = json.load(f)
        zip_code_list = json_zips['zips']

    df = df[df['RegionName'].isin(zip_code_list)]

    # Data X: all zips, Data Y: Price from most recent month
    most_recent_column = df.columns[-1]
    new_columns = ['RegionName', most_recent_column]
    df = df[new_columns]

    # Filter out rows with NaN values
    df = df.dropna()

    # Convert zip codes to strings, prices to ints
    df['RegionName'] = df['RegionName'].apply(str)
    df[most_recent_column] = df[most_recent_column].apply(int)

    # Load DB config
    with open("db_config.json") as g:
        db_config = json.load(g)

        db_database = db_config['database']
        db_user = db_config['user']
        db_password = db_config['password']
        db_host = db_config['host']
        db_port = db_config['port']

    # Connect to DB
    db_conn = psycopg2.connect(
        database=db_database,
        user=db_user,
        password=db_password,
        host=db_host,
        port=db_port
    )

    cur = db_conn.cursor()

    # Make SQL insert for each row in DF
    for i in range(len(df)):
        row = df.iloc[i,:]
        zip_code = row['RegionName']

        # Insert into DB
        qry = "INSERT INTO datasets.housing_median_sale_price_current VALUES ({}, {})".format(
            zip_code, row[most_recent_column]
        )

        # If row already exists for unique zip, update price of that zip
        upsert = "ON CONFLICT (zip) DO UPDATE SET sale_price = {}".format(
            row[most_recent_column]
        )

        qry_with_upsert = qry + " " + upsert

        cur.execute(qry_with_upsert)

    # Commit changes to DB
    db_conn.commit()

    # Clean up
    cur.close()
    db_conn.close()