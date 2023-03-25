from flask import Flask, request, send_file
import os

app = Flask(__name__)

@app.route('/get_new_question', methods=['GET'])
def subir_archivo():
    print

if __name__ == '__main__':
    app.run(host='0.0.0.0', debug=True)