from flask import Flask, jsonify
from quiz import getNewQuestion
app = Flask(__name__)

@app.route('/get_new_question', methods=['GET'])
def subir_archivo():
    return getNewQuestion()

if __name__ == '__main__':
    app.run(host='0.0.0.0', debug=True, port=6312)