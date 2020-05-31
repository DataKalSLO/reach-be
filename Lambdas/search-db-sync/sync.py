import psycopg2
from elasticsearch import Elasticsearch
import json

def convert_story_to_dict(obj):
    res = {
        'story_id': obj[0],
        'title': obj[3]
    }

    return res

def convert_graph_to_dict(obj):
    res = {
        'graph_id': obj[0],
        'title': obj[1]
    }

    return res

def test_query(es_client, query):
    search_results = es_client.search(
        index='testall',
        body={
            'query': {
                'match': {
                    'title': query,
                }
            }
        }
    )

    print(search_results)

def see_all_docs(es_client):
    search_results = es_client.search(
        index='_all',
        body={
            'query': {
                'match_all': {}
            }
        }
    )

    print(search_results)

def handler(event, context):

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

    print(db_conn)
    cur = db_conn.cursor()

    cur.execute("SELECT * FROM public.story;")
    rows = cur.fetchall() 
    dicts = []
    for row in rows:
        res = convert_story_to_dict(row)
        dicts.append(res)
    #print(dicts)

    cur.execute("SELECT * FROM public.graph")
    rows = cur.fetchall()
    dicts2 = []
    for row in rows:
        res = convert_graph_to_dict(row)
        dicts2.append(res)
    #print(dicts2)


    es_client = Elasticsearch(['https://search-hourglass-search-test-boatibipr2tvrekti6tuz7pghi.us-east-2.es.amazonaws.com'])
    print(es_client)

    # Need to delete existing indices so that deleted stories/graphs won't persist
    es_client.indices.delete(index='stories', ignore=[400, 404])
    es_client.indices.delete(index='graphs', ignore=[400, 404])

    # test index
    '''
    es_client.index(index='testall', id='123thisisatest321', body={
        'title': 'I am using Python with Elasticsearch this'
    })
    '''


    # Store stories, graphs in respective indices
    for story in dicts:
        es_client.index(index='stories', id=story['story_id'], body={
            'title': story['title']
        })

    for graph in dicts2:
        es_client.index(index='graphs', id=graph['graph_id'], body={
            'title': graph['title']
        })


    #test_query(es_client, "title")
    #see_all_docs(es_client)

    print("Success")


    cur.close()
    db_conn.close()