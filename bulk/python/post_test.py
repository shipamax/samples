import requests
import time
import json
import os
import argparse
import uuid


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--token', type=str, required=True,
                        help='User access token from API')
    parser.add_argument('--file', type=str, required=True,
                        help='send e-mail from file')
    args = parser.parse_args()

    domain = 'https://sisyphus.shipamax.com'
    endpoint = '/api/Messages/push'
    custom_id = str(uuid.uuid4())  # free to be set by user
    start = time.time()
    with open(args.file, 'r') as myfile:
        data = myfile.read()
    url = domain + endpoint
    headers = {
        'Content-Type': 'application/json',
    }
    params = {
        'access_token': args.token
    }
    payload = {
        'raw': data,
        'update': True
    }
    if custom_id != '':
        payload['customId'] = custom_id

    response = requests.post(url, params=params, headers=headers,
                             data=json.dumps(payload))
    res = response.json()
    print(res)

    end = time.time()
    print('time: ', end - start)
