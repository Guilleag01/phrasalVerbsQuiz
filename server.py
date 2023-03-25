from flask import Flask, request, send_file
import os

app = Flask(__name__)

@app.route('/subir-archivo', methods=['POST'])
def subir_archivo():
    for filename in os.listdir('./'):
        if os.path.splitext(filename)[0] == 'video':
            os.remove(filename)
    archivo = request.files['archivo']
    ruta_archivo = './video.' + archivo.filename.split('.')[-1]
    archivo.save(ruta_archivo)
    app.logger.info("Enviando")
    ret = send_file('base.txt', as_attachment=True)
    app.logger.info("Enviado!")
    return ret

if __name__ == '__main__':
    app.run(host='0.0.0.0', debug=True)