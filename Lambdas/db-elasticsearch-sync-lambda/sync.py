import psycopg2
from elasticsearch import Elasticsearch
import json

def convert_story_to_dict(obj):
    res = {
        'story_id': obj[0],
        'user_id': obj[1],
        'publication_status': obj[2],
        'title': obj[3],
        'description': obj[4],
        'date_created': obj[5],
        'date_last_edited': obj[6],
        'user_name': obj[8]
    }

    return res

def convert_graph_to_dict(obj):
    res = {
        'graph_id': obj[0],
        'graph_title': obj[1],
        'user_id': obj[2],
        'timestamp': obj[3],
        'snapshot_url': obj[4],
        'user_name': obj[7]
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

    cur = db_conn.cursor()

    cur.execute("SELECT * FROM public.story LEFT JOIN public.person ON public.story.user_id = public.person.email;")
    rows = cur.fetchall() 
    dicts = []
    for row in rows:
        res = convert_story_to_dict(row)
        dicts.append(res)

    cur.execute("SELECT * FROM public.graph LEFT JOIN public.person ON public.graph.user_id = public.person.email;")
    rows = cur.fetchall()
    dicts2 = []
    for row in rows:
        res = convert_graph_to_dict(row)
        dicts2.append(res)


    es_client = Elasticsearch(['REPLACE_WITH_ELASTICSEARCH_URL'])
    
    # Need to delete existing indices so that deleted stories/graphs won't persist
    es_client.indices.delete(index='stories', ignore=[400, 404])
    es_client.indices.delete(index='graphs', ignore=[400, 404])


    # Store stories, graphs in respective indices
    for story in dicts:
        es_client.index(index='stories', id=story['story_id'], body={
            'user_id': story['user_id'],
            'publication_status': story['publication_status'],
            'title': story['title'],
            'description': story['description'],
            'date_created': story['date_created'],
            'date_last_edited': story['date_last_edited'],
            'user_name': story['user_name']
        })

    for graph in dicts2:
        es_client.index(index='graphs', id=graph['graph_id'], body={
            'title': graph['graph_title'],
            'user_id': graph['user_id'],
            'timestamp': graph['timestamp'],
            'snapshot_url': graph['snapshot_url'],
            'user_name': graph['user_name']
        })


    cur.close()
    db_conn.close()