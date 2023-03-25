import json
import random
import re

def main():
    db:dict
    with open("db.json") as jfile:
        db = json.load(jfile)
    pv: str
    example:str
    possibleWords = []
    while(len(possibleWords) == 0):
        try: 
            pv = random.choice(list(db.keys()))
            # print(pv)
            
            example = random.choice(db[pv]['examples'])
            # print(example)

            possibleWords = [w for w in re.split(r'\.|,|;|:|\*|\n|\\|\ ', example) if w in db[pv]['words']]
            # print(possibleWords)
        except:
            continue
        
    word = random.choice(possibleWords)

    otherWithSameVerb = [k for k in list(db.keys()) if db[pv]['words'][0] == db[k]['words'][0]]
    # print(otherWithSameVerb)
    
    possibleOptions = []
    
    # Añadimos la correcta
    possibleOptions.append(word)
    
    # La posición de la palabra en el pv para que tengan mas sentido las opciones
    pos = db[pv]['words'].index(word)

    if pos > 0:
        pos = 1

    # Elegimos primero palabras de phrasal verbs similares para que tenga mas sentido
    while(len(possibleOptions) < 4 and len(otherWithSameVerb) > 0):
        subtitute = random.choice(otherWithSameVerb)
        option = db[subtitute]['words'][pos]
        otherWithSameVerb.remove(subtitute)
        if option not in possibleOptions:
            possibleOptions.append(option)

    # print(possibleOptions)

    # Si no hay suficientes se escogen aleatoriamente del resto
    while(len(possibleOptions) < 4):
        option = word
        while(option in possibleOptions):
            subtitute = random.choice(list(db.keys()))
            option = db[subtitute]['words'][pos]
        possibleOptions.append(option)
    
    # print(possibleOptions)
    
    


    formattedExample = example.replace(" " + word, " ____")
    print(formattedExample)
    for i in range(len(possibleOptions)):
        print(str(i + 1) + ". " + possibleOptions[i])
            

def getNewQuestion() -> str:
    db:dict
    with open("db.json") as jfile:
        db = json.load(jfile)
    pv: str
    example:str
    possibleWords = []
    while(len(possibleWords) == 0):
        try: 
            pv = random.choice(list(db.keys()))
            # print(pv)
            
            example = random.choice(db[pv]['examples'])
            # print(example)

            possibleWords = [w for w in re.split(r'\.|,|;|:|\*|\n|\\|\ ', example) if w in db[pv]['words']]
            # print(possibleWords)
        except:
            continue
        
    word = random.choice(possibleWords)

    otherWithSameVerb = [k for k in list(db.keys()) if db[pv]['words'][0] == db[k]['words'][0]]
    # print(otherWithSameVerb)
    
    possibleOptions = []
    
    # Añadimos la correcta
    possibleOptions.append(word)
    
    # La posición de la palabra en el pv para que tengan mas sentido las opciones
    pos = db[pv]['words'].index(word)

    if pos > 0:
        pos = 1

    # Elegimos primero palabras de phrasal verbs similares para que tenga mas sentido
    while(len(possibleOptions) < 4 and len(otherWithSameVerb) > 0):
        subtitute = random.choice(otherWithSameVerb)
        option = db[subtitute]['words'][pos]
        otherWithSameVerb.remove(subtitute)
        if option not in possibleOptions:
            possibleOptions.append(option)

    # print(possibleOptions)

    # Si no hay suficientes se escogen aleatoriamente del resto
    while(len(possibleOptions) < 4):
        option = word
        while(option in possibleOptions):
            subtitute = random.choice(list(db.keys()))
            option = db[subtitute]['words'][pos]
        possibleOptions.append(option)
    
    random.shuffle(possibleOptions)

    response = {
        "question": example.replace(" " + word, " ____"),
        "option0": possibleOptions[0],
        "option1": possibleOptions[1],
        "option2": possibleOptions[2],
        "option3": possibleOptions[3],
        "solution": possibleOptions.index(word)
    }
    
    jstring = json.dumps(response, ensure_ascii=False)
    return jstring


if __name__ == "__main__":
    main()