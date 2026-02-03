import json
from flask import Flask, jsonify, request

app = Flask(__name__)


@app.route('/employees', methods=['GET'])

def get_employees():
    return jsonify([{'id':1}])

if __name__ == '__main__':
    app.run(port=5000)
