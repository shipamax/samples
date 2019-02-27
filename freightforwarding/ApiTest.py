import requests
import json
import argparse
import uuid


HOST = 'http://developer.shipamax-api.com'
BILL_OF_LADING_ID = 1


def login(host, username, password):
    url = host + '/api/v1/users/login'
    headers = {
        'Content-Type': 'application/json',
    }
    payload = {
        'username': username,
        'password': password
    }
    response = requests.post(url,
                             headers=headers,
                             data=json.dumps(payload)
                             )
    if response.status_code != 200:
      raise ValueError('Login failed')
    access_token = response.json()['id']
    return access_token


def logout(host, token):
    url = host + '/api/v1/users/logout'
    headers = {
        'Content-Type': 'application/json',
    }
    params = {
        'access_token': token
    }
    response = requests.post(url,
                             headers=headers,
                             params=params
                             )
    if response.status_code != 204:
        raise ValueError('Logout failed')


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--username', type=str)
    parser.add_argument('--password', type=str)
    parser.add_argument('--host', type=str)
    args = parser.parse_args()

    if args.host:
        host = args.host
    else:
        host = HOST

    token = login(host, args.username, args.password)
    logout(host, token)
    print ('Test complete')
