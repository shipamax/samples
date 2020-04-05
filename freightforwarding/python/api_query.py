import requests
import json
import argparse
import uuid
from util import login, logout


DEFAULT_HOST = 'https://developer.shipamax-api.com'


def query(_host, custom_id, _token):
    """ Query parsing result """
    url = '{}{}'.format(_host, '/api/v1/DocumentContainers/query')
    custom_id_json = '["{}"]'.format(custom_id)
    headers = {
        'Content-Type': 'application/json'
    }
    params = {
        'customIds': custom_id_json,
        'access_token': _token
    }
    response = requests.get(url, params=params, headers=headers)
    if (response.status_code != 200):
        raise Exception('Query failed. Code {}'.format(response.status_code))
    print(response.content)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--username', type=str, required=True)
    parser.add_argument('--password', type=str, required=True)
    parser.add_argument('--host', type=str)
    parser.add_argument('--custom_id', type=str, required=True)
    args = parser.parse_args()

    if args.host:
        host = args.host
    else:
        host = DEFAULT_HOST

    _token = login(host, args.username, args.password)
    query(host, args.custom_id, _token)
    logout(host, _token)


if __name__ == '__main__':
    main()
