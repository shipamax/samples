import requests
import json
import argparse
import uuid
from util import login, logout


DEFAULT_HOST = 'https://developer.shipamax-api.com'
BILL_OF_LADING_ID = 1


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
    if (response.status_code != 200):
        raise Exception('Upload failed')    
    print(response.content)


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
