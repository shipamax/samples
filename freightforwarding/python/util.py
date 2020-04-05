import requests
import json


DEFAULT_HOST = 'https://developer.shipamax-api.com'


def login(_host, username, password):
    """ Log in to API """
    url = _host + '/api/v1/users/login'
    headers = {
        'Content-Type': 'application/json',
    }
    payload = {
        'email': username,
        'password': password
    }
    response = requests.post(url, headers=headers, data=json.dumps(payload))
    result_json = response.json()
    if (response.status_code != 200) or ('id' not in result_json):
        raise ValueError('Login failed')
    access_token = result_json['id']
    return access_token


def logout(_host, _token):
    """ Log out after use """
    url = _host + '/api/v1/users/logout'
    headers = {
        'Content-Type': 'application/json',
    }
    params = {
        'access_token': _token
    }
    response = requests.post(url, headers=headers, params=params)
    if not response.status_code == 204:
        raise ValueError('Logout failed')

