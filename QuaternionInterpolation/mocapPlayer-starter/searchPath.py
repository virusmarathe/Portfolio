f = open('bezierQuatDanceN20.amc', 'r')
outFile = open('data.txt', 'w')
for i in range(4): f.next()
for line in f:
    if line.startswith("lfemur"):
        groups = line.split(' ')
        outFile.write(groups[1]+"\n")
