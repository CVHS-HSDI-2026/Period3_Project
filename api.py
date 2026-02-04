import json
import os
from flask import Flask, flash, request, redirect, url_for,jsonify, send_from_directory
from werkzeug.utils import secure_filename

UPLOAD_FOLDER = '/home/potato/Downloads'
ALLOWED_EXTENSIONS = {'txt', 'pdf', 'png', 'jpg', 'jpeg', 'gif'}
secret_key = os.urandom(24).hex() 
app = Flask(__name__)
app.secret_key = secret_key

app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER
def allowed_file(filename):
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS
def audio_splicing(file):
    return file
@app.route('/employees', methods=['GET'])

def get_employees():
    return jsonify([{'id':1}])



@app.route('/', methods=['GET', 'POST'])
def upload_file():
    if request.method == 'POST':
        # check if the post request has the file part
        if 'file' not in request.files:
            flash('No file part')
            return redirect(request.url)
        file = request.files['file']
        # If the user does not select a file, the browser submits an
        # empty file without a filename.
        if file.filename == '':
            flash('No selected file')
            return redirect(request.url)
        if file and allowed_file(file.filename):
            filename = secure_filename(file.filename)
            file = audio_splicing(file)
            file.save(os.path.join(app.config['UPLOAD_FOLDER'], filename))
            return send_from_directory(app.config["UPLOAD_FOLDER"], filename)
    return send_from_directory(app.config["UPLOAD_FOLDER"], file.filename)

if __name__ == '__main__':
    app.run(port=5000)
