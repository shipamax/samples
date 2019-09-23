import requests
import json
import argparse
import uuid


DEFAULT_HOST = 'https://developer.shipamax-api.com'
BILL_OF_LADING_ID = 1


def login(_host, username, password):
    """ Log in to API """
    url = _host + '/api/v1/users/login'
    headers = {
        'Content-Type': 'application/json',
    }
    payload = {
        'username': username,
        'password': password
    }
    response = requests.post(url, headers=headers, data=json.dumps(payload))
    result_json = response.json()
    if 'id' not in result_json:
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


def upload(_host, filename, file_type, _token):
    """ Upload document """
    url = _host + '/api/v1/DocumentContainers/upload'
    data = {
        'access_token': _token,
        'customId': str(uuid.uuid4()),
        'type': file_type
    }

    files = {'file': (filename, open(filename, 'rb'))}
    response = requests.post(url, params=data, files=files)
    print(response)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--username', type=str, required=True)
    parser.add_argument('--password', type=str, required=True)
    parser.add_argument('--host', type=str)
    parser.add_argument('--file', type=str, required=True,
                        help='send e-mail from file')
    parser.add_argument('--type', type=int, default=BILL_OF_LADING_ID,
                        help='send e-mail from file')
    args = parser.parse_args()

    if args.host:
        host = args.host
    else:
        host = DEFAULT_HOST

    _token = login(host, args.username, args.password)
    upload(host, args.file, args.type, _token)
    logout(host, _token)


if __name__ == '__main__':
    main()
