print("########### 1 - IF ELIF ELSE ##########")
a = 4
if a == 1 do
        print("IF1 a="+a)
elif a == 2 do
        print("ELIF2 a="+a)
elif a == 3 do
        print("ELIF3 a="+a)
elif a == 4 do
        print("ELIF4 a="+a)
else
        print("ELSE5 a="+a)
end

print("\n########### 2 - ANINHAMENTO ###########")
a = 4
if a == 1 do
        print("IF1 a="+a)
elif a == 2 do
        print("ELIF2 a="+a)
elif a == 3 do
        print("ELIF3 a="+a)
else
        if a == 1 do
                print("IF41 a="+a)
        elif a == 2 do
                print("ELIF42 a="+a)
        elif a == 3 do
                print("ELIF43 a="+a)
        else
                print("ELSE44 a=" + a)
        end
        print("ELSE4 a="+a)
end

print("\n########### 3 - ANINHAMENTO ###########")
a = 3
if a == 1 do
        print("IF1 a="+a)
elif a == 2 do
        print("ELIF2 a="+a)
elif a == 3 do
        if a == 1 do
                print("IF31 a="+a)
        elif a == 2 do
                print("ELIF32 a="+a)
        elif a == 3 do
                print("ELIF33 a="+a)
        else
                print("ELSE34 a=" + a)
        end
        print("ELIF3 a="+a)
else
        print("ELSE4 a="+a)
end

print("\n########### 4 - ANINHAMENTO ###########")
a = 3
if a == 1 do
	print("IF1 a="+a)
elif a == 3 do
	print("ELIF2 a="+a)
	if a == 1 do
		print("XIF21 a=" + a)
	elif a == 3 do
		print("ELIF22 a=" + a)
	else
		print("ELSE23 a=" + a)
	end
else
	print("ELSE4 a=" + a)
end

print("\n########### 5 - ANINHAMENTO ###########")
a = 3
if a == 1 do
	print("IF1 a="+a)
else
	print("ELSE2 a="+a)
	if a == 1 do
		print("IF21 a=" + a)
	elif a == 3 do
		print("ELIF22 a=" + a)
	else
		print("ELSE23 a=" + a)
	end
end

# include("if_test.c")
